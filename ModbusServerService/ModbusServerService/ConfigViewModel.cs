using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public class ConfigViewModel : Notify
    {
        public string _webAdress;
        public string _port;
        public string _modbusServerAdress;
        public int _duration;
        private Register _selectedRegister;
        public ObservableCollection<Register> _registers;

        public string WebAdress { get => _webAdress; set { _webAdress = value; OnPropertyChanged(); } }
        public string Port { get => _port; set { _port = value; OnPropertyChanged(); } }
        public string ModbusServerAdress { get => _modbusServerAdress; set { _modbusServerAdress = value; OnPropertyChanged(); } }
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(); } }

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
        private bool CanDelete
        {
            get { return SelectedRegister != null; }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(param => DeleteSelected(), param => CanDelete);
                }
                return _deleteCommand;
            }
        }

        public ConfigViewModel()
        {
            Registers = new ObservableCollection<Register>();
        }

        private void DeleteSelected()
        {
                Registers.Remove(SelectedRegister);
        }
    }
}
