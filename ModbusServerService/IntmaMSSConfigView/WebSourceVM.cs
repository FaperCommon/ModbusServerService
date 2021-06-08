using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public class WebSourceVM : TreeViewItem
    {
        WebSource _webSource;
        private RegistersGroupVM _selectedGroup;

        override public string Name { get => _webSource.Name; set { _webSource.Name = value; OnPropertyChanged(); } }
        public int Duration { get => _webSource.Duration; set => _webSource.Duration = value; }
        public string WebAddress { get => _webSource.WebAddress; set => _webSource.WebAddress = value; }
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

            _webSource = webSource;
            Childs = new BindingList<TreeViewItem>();
            Childs.ListChanged += Childs_ListChanged;
            foreach (var group in _webSource.RegistersGroups)
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
                File.WriteAllText(saveFileDialog.FileName, _webSource.ExportTopic(type));
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
        }

        override protected void AddChild()
        {
            var group = new RegistersGroup();
            var groupVM = new RegistersGroupVM(group);
            var wA = new Windows.AddRegGroupWindow(groupVM);
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                _webSource.RegistersGroups.Add(group);
                Childs.Add(wA.AddedRegistersGroup);
            }
        }

        override public object Clone()
        {
            return new WebSourceVM((WebSource)_webSource.Clone());
        }
    }
}
