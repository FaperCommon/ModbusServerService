using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class ConfigViewModel : Notify
    {
        string _port;
        string _modbusServerAdress;
        ObservableCollection<WebSourceViewModel> _webSources;
        int _duration;
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(); } }
        public string Port { get => _port; set { _port = value; OnPropertyChanged(); } }
        public string ModbusServerAddress { get => _modbusServerAdress; set { _modbusServerAdress = value; OnPropertyChanged(); } }
        public ObservableCollection<WebSourceViewModel> Childs { get => _webSources; set { _webSources = value; OnPropertyChanged(); } }
        public string Type { get; } = "ConfigViewModel";

        public ConfigViewModel()
        {
            Port = "502";
            ModbusServerAddress = "0.0.0.0";
            Duration = 5;
            Childs = new ObservableCollection<WebSourceViewModel>();
        }
        WebSourceViewModel _selectedSource;
        public WebSourceViewModel SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                OnPropertyChanged();
            }
        }
        private void DeleteSelected()
        {
            Childs.Remove(SelectedSource);
        }
        private void EditSelected()
        {
            //var wA = new EditWebSourceWindow(SelectedSource);
            //wA.ShowDialog();
        }
        private void AddIntoSelected()
        {
            var wA = new Windows.AddRegGroupWindow();
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                SelectedSource.Childs.Add(wA.AddedRegistersGroup);
            }
        }
        private void AddGroup()
        {
            MessageBox.Show("WebDelete");
        }
        private ICommand _registersGroupAddCommand;
        public ICommand RegistersGroupAddCommand
        {
            get
            {
                if (_registersGroupAddCommand == null)
                {
                    _registersGroupAddCommand = new RelayCommand(param => AddGroup(), param => true/*CanOperate*/);
                }
                return _registersGroupAddCommand;
            }
        }
        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param => AddIntoSelected(), param => CanOperate);
                }
                return _addCommand;
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(param => EditSelected(), param => CanOperate);
                }
                return _editCommand;
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(param => DeleteSelected(), param =>CanOperate);
                }
                return _deleteCommand;
            }
        }
        private bool CanOperate
        {
            get { return SelectedSource != null; }
        }


        public void ConfingRead(string filepath)
        {
            var doc = XDocument.Load(filepath);
            var el = doc.Element("Config");
            ModbusServerAddress = el.Element("ModbusServerAddress").Value;
            Port = el.Element("Port").Value;
            Duration = Int32.Parse(el.Element("Duration").Value);
            Childs = new ObservableCollection<WebSourceViewModel>();
            foreach (var source in el.Element("Sources").Elements())
            {
                Childs.Add(new WebSourceViewModel(source));
            }
        }

        public void UpdateConfig(string filepath)
        {
            XElement contacts =
                new XElement("Config",
                    new XElement("Port", $"{Port}"),
                    new XElement("Duration", $"{Duration}"),
                    new XElement("ModbusServerAddress", $"{ModbusServerAddress}"),
                    new XElement("Sources",
                        Childs.Select((source, i) => new XElement($"Source{i}",
                            new XElement("Duration", source.Duration),
                            new XElement("Name", source.Name),
                            new XElement("WebAddress", source.WebAddress),
                            new XElement("Weights", source.Childs.Select((weight, j) => new XElement($"Weight{j}",
                                new XElement("Name", weight.Name),
                                new XElement("Registers", weight.Registers.Select((reg, k) => new XElement($"Reg{k}",
                                    new XElement("NeedTwoRegisters", reg.NeedTwoRegisters),
                                    new XElement("ValueRegister", reg.ValueRegister),
                                    new XElement("DataType", reg.SelectedDataType),
                                    new XElement("Path",
                                    reg.Path.Split(Register.PathDel).Select(path => new XElement(path, path)))))))))))));

            XDocument s = new XDocument(contacts);
            s.Save(filepath);
        }

    }
}
