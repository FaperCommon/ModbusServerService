using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class Register : Notify
    {
        public char PathDel { get; set; } = ',';
        public Register(XElement reg)
        {
            Path = string.Join(PathDel.ToString(), reg.Element("Path").Elements().Select(a => a.Value));
            SelectedDataType = reg.Element("DataType").Value;
            ValueRegister = Int32.Parse(reg.Element("ValueRegister").Value);
            MantissaRegister = Int32.Parse(reg.Element("MantissaRegister").Value);
            IsFloat = Boolean.Parse(reg.Element("IsFloat").Value);
        }
        public Register()
        {

        }

        public bool IsFloat { get; set; }

        public ObservableCollection<string> _dataType = new ObservableCollection<string>(new string[] { "Word", "Float", "Int" });
        public ObservableCollection<string> DataType { get => _dataType; set { _dataType = value; OnPropertyChanged(); } }

        private string _selectedDataType;
        public string SelectedDataType
        {
            get { return _selectedDataType; }
            set
            {
                if (value == "Float")
                    IsFloat = true;
                else IsFloat = false;

                _selectedDataType = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get { return _path == null ? "" : String.Join(PathDel.ToString(), _path); }
            set { _path = new List<string>(value.Split(new char[] { PathDel })); OnPropertyChanged(); }
        }
        public List<string> _path { get; set; }
        public int ValueRegister { get => _valueRegister; set { _valueRegister = value; OnPropertyChanged(); } }
        public int _valueRegister;
        public int MantissaRegister { get; set; }
        public int Value { get; set; }
        public int Mantissa { get; set; }
    }
}
