using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public class WebSourceVM : TreeViewItem
    {
        public WebSource WebSource { get; private set; }
        private RegistersGroupVM _selectedGroup;

        override public string Name { get => WebSource.WebAddress; set { WebSource.WebAddress = value; OnPropertyChanged(); } }
        public int Duration { get => WebSource.Duration; set => WebSource.Duration = value; }
        public string WebAddress { get => WebSource.WebAddress; set => WebSource.WebAddress = value; }
        public RegistersGroupVM SelectedGroup { get => _selectedGroup; set { _selectedGroup = value; OnPropertyChanged(); } }

        public WebSourceVM(WebSource webSource)
        {
            Actions = new ObservableCollection<ContextAction>() {
                new ContextAction() { Name = "Добавить группу", Action = AddChildCommand },
                new ContextAction() { Name = "Дублировать", Action = DublicateNodeCommand },
                new ContextAction() { Name = "Выгрузить в CSV (KepServer)", Action = ExportForKepServerCommand },
                new ContextAction() { Name = "Выгрузить в CSV (Suitelink)", Action = ExportTopicForSuitelinkCommand },
                new ContextAction() { Name = "Удалить", Action = DeleteNodeCommand },
                new ContextAction() { Name = "Свойства", Action = PropertyShowCommand },
            };

            WebSource = webSource;
            Childs = new BindingList<TreeViewItem>();
            Childs.ListChanged += Childs_ListChanged;
            foreach (var group in WebSource.RegistersGroups)
            {
                Childs.Add(new RegistersGroupVM(group));
            }
        }

        private void Childs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.NewIndex < Childs.Count && Childs.Count > 0)
            {
                if (Childs[e.NewIndex].NeedDelete)
                {
                    Childs.Remove(Childs[e.NewIndex]);
                    SelectedGroup = null;
                }
                else if (Childs[e.NewIndex].NeedDublicate)
                {
                    Childs[e.NewIndex].NeedDublicate = false;
                    Childs.Insert(e.NewIndex + 1, (RegistersGroupVM)Childs[e.NewIndex].Clone());
                }
                else
                {
                    SelectedGroup = (RegistersGroupVM)Childs[e.NewIndex];
                }
            }
            else if (Childs.Count == 0)
            {
                SelectedGroup = null;
            }
        }

        private ICommand _exportForKepServerCommand;
        public ICommand ExportForKepServerCommand
        {
            get
            {
                if (_exportForKepServerCommand == null)
                {
                    _exportForKepServerCommand = new RelayCommand(param => ExportTopic(TopicType.KeepServer), param => true);
                }
                return _exportForKepServerCommand;
            }
        }

        private void ExportTopic(TopicType type)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv) | *.csv";
            saveFileDialog.DefaultExt = "csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, WebSource.ExportTopic(type));
            }
        }

        private ICommand _exportTopicForSuitelinkCommand;
        public ICommand ExportTopicForSuitelinkCommand
        {
            get
            {
                if (_exportTopicForSuitelinkCommand == null)
                {
                    _exportTopicForSuitelinkCommand = new RelayCommand(param => ExportTopic(TopicType.Suitelink), param => true);
                }
                return _exportTopicForSuitelinkCommand;
            }
        }

        override protected void PropertyShow()
        {
            var wA = new Windows.AddWebSourceWindow(this);
            wA.ShowDialog();
            OnPropertyChanged("Name");
        }

        override protected void AddChild()
        {
            var group = new RegistersGroup();
            var groupVM = new RegistersGroupVM(group);
            var wA = new Windows.AddRegGroupWindow(groupVM);
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                if (WebSource.RegistersGroups.Any(a => a.Name == group.Name))
                {
                    MessageBox.Show("Источник уже добавлен!");
                    return;
                }

                WebSource.RegistersGroups.Add(group);
                Childs.Add(wA.AddedRegistersGroup);
            }
        }

        override public object Clone()
        {
            return new WebSourceVM((WebSource)WebSource.Clone());
        }
    }
}
