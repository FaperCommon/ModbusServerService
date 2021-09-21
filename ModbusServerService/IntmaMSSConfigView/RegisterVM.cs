using System.Collections.Generic;

namespace Intma.ModbusServerService.Configurator
{
    public class RegisterVM : Notify
    {
        public Register Register { get; private set; }

        public int ValueRegister { get => Register.ValueRegister; set { Register.ValueRegister = value; OnPropertyChanged(); OnPropertyChanged("SecondRegister"); } }

        public string DataType { get => Register.DataType; set { Register.DataType = value; OnPropertyChanged("SecondRegister"); } }

        public static IReadOnlyList<string> MbtcpDataTypes { get => DataTypes.MbtcpDataTypes; }

        public int SecondRegister { get => Register.SecondRegister; }

        public string Path { get => Register.Path; set => Register.Path = value; }
        
        public RegisterVM(Register register)
        {
            Register = register;
        }
        
        public object Clone()
        {
            return Register.Clone();
        }
    }
}
