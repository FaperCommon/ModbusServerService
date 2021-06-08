using System.Collections.Generic;

namespace Intma.ModbusServerService.Configurator
{
    public class RegisterVM : Notify
    {
        Register _register;

        public int ValueRegister { get => _register.ValueRegister; set { _register.ValueRegister = value; OnPropertyChanged(); OnPropertyChanged("SecondRegister"); } }

        public string DataType { get => _register.DataType; set { _register.DataType = value; OnPropertyChanged("SecondRegister"); } }

        public static IReadOnlyList<string> MbtcpDataTypes { get => DataTypes.MbtcpDataTypes; }

        public int SecondRegister { get => _register.SecondRegister; }

        public string Path { get => _register.Path; set => _register.Path = value; }
        
        public RegisterVM(Register register)
        {
            _register = register;
        }
        
        public object Clone()
        {
            return _register.Clone();
        }
    }
}
