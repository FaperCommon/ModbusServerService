using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class WebSourceViewModel: Notify
    {
        string _webAdress;
        Register _selectedRegister;
        ObservableCollection<Register> _registers;
        int _duration;
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(); } }
        public string WebAddress { get => _webAdress; set { _webAdress = value; OnPropertyChanged(); } }

        public ObservableCollection<Register> Registers { get => _registers; set { _registers = value; OnPropertyChanged(); } }
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set
            {
                _selectedRegister = value;
                OnPropertyChanged();
            }
        }

        public WebSourceViewModel()
        {
            Duration = 5;
            Registers = new ObservableCollection<Register>();
        }
        public WebSourceViewModel(XElement source)
        {
            WebAddress = source.Element("WebAddress").Value;
            Duration = Int32.Parse(source.Element("Duration").Value);
            Registers = new ObservableCollection<Register>();
            foreach (var reg in source.Element("Registers").Elements())
            {
                Registers.Add(new Register(reg));
            }
        }
        private bool CanOperate
        {
            get { return SelectedRegister != null; }
        }
        private void DeleteSelected()
        {
            Registers.Remove(SelectedRegister);
        }
        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(param => DeleteSelected(), param => CanOperate);
                }
                return _deleteCommand;
            }
        }
    }
}
