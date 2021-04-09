using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class Register: INotifyPropertyChanged
    {
        public char PathDel { get; set; } = ',';
        public Register(XElement reg)
        {
            Path = string.Join(",", reg.Element("Path").Elements().Select(a => a.Value));
            DataType = reg.Element("DataType").Value;
            ValueRegister = Int32.Parse(reg.Element("ValueRegister").Value);
            MantissaRegister = Int32.Parse(reg.Element("MantissaRegister").Value);
            IsFloat = Boolean.Parse(reg.Element("IsFloat").Value);
        }
        public Register()
        {

        }
        public bool IsFloat { get; set; }
        public string DataType { get => _dataType; set { _dataType = value; OnPropertyChanged(); } }
        public string _dataType;
        public string Path {
            get => String.Join(",",_path);
            set { _path = new List<string>(value.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)); OnPropertyChanged(); } }
        public List<string> _path { get; set; }
        public int ValueRegister { get => _valueRegister; set { _valueRegister = value; OnPropertyChanged(); } }
        public int _valueRegister;
        public int MantissaRegister { get; set; }
        public int Value { get; set; }
        public int Mantissa { get; set; }

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
