using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class Register : Notify, ICloneable
    {
        static public char PathDel { get; set; } = ',';

        private int _valueRegister;
        private string _dataType;

        public int ValueRegister { get => _valueRegister; set { _valueRegister = value; OnPropertyChanged("SecondRegister"); OnPropertyChanged(); } }

        public static IReadOnlyList<string> DataTypes { get; } = new List<string>() { "Float", "Int", "Short" }; 

        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (value == "Float" || value == "Int")
                    NeedTwoRegisters = true;
                else NeedTwoRegisters = false;

                _dataType = value;
                OnPropertyChanged("SecondRegister");
                OnPropertyChanged();
            }
        }

        public int SecondRegister { get { return NeedTwoRegisters? ValueRegister + 1 : -1; } }

        public string Path { get; set; }

        public object Value { get; set; }

        public bool NeedTwoRegisters { get; set; }

        public Register(XElement reg)
        {
            Path = string.Join(PathDel.ToString(), reg.Element("Path").Elements().Select(a => a.Value));
            DataType = reg.Element("DataType").Value;
            ValueRegister = Int32.Parse(reg.Element("ValueRegister").Value);
            NeedTwoRegisters = Boolean.Parse(reg.Element("NeedTwoRegisters").Value);
        }

        public Register()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"\"{Path}\",\"{400000 + ValueRegister}\"";  //Server have a registers size is 6 digit, writing into holding registers
        }

        public string ToString(string parent)
        {
            return $"\"{parent}{Path.Replace(",","")}\",\"{400000 + ValueRegister}\"";
        }
    }
}
