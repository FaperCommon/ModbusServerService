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
    public class Register : ICloneable
    {

        private string _dataType;
        static public char PathDel { get; set; } = ',';
        public int ValueRegister { get; set; }
        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (value == "Float" || value == "Int")
                    NeedTwoRegisters = true;
                else NeedTwoRegisters = false;

                _dataType = value;
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

        /// <summary>
        /// Server have a registers size is 6 digit, writing into holding registers
        /// </summary>
        /// <returns>"Port.Tag","400000"</returns>
        public override string ToString()
        {
            return $"\"{Path}\"{PathDel}\"{400000 + ValueRegister}\"";  
        }
        /// <summary>
        /// Server have a registers size is 6 digit, writing into holding registers
        /// parent - unic groupName, just for show
        /// </summary>
        /// <returns>"Port.Tag","400000"</returns>
        public string ToString(string parent)
        {
            return $"\"{parent}{Path.Replace(",","")}\"{PathDel}\"{400000 + ValueRegister}\"";
        }
    }
}
