﻿using System;
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
        public ObservableCollection<string> _dataType = new ObservableCollection<string>(new string[] { "Float", "Int","Short" });
        private List<string> _path;
        private string _selectedDataType;

        public int ValueRegister { get => _valueRegister; set { _valueRegister = value; OnPropertyChanged("SecondRegister"); OnPropertyChanged(); } }

        public ObservableCollection<string> DataType { get => _dataType; set { _dataType = value; OnPropertyChanged(); } }

        public string SelectedDataType
        {
            get { return _selectedDataType; }
            set
            {
                if (value == "Float" || value == "Int")
                    NeedTwoRegisters = true;
                else NeedTwoRegisters = false;

                _selectedDataType = value;
                OnPropertyChanged("SecondRegister");
                OnPropertyChanged();
            }
        }

        public int SecondRegister { get { return NeedTwoRegisters? ValueRegister + 1 : -1; } }

        public string Path
        {
            get { return _path == null ? "" : String.Join(PathDel.ToString(), _path); }
            set { _path = new List<string>(value.Split(new char[] { PathDel })); OnPropertyChanged(); }
        }

        public object Value { get; set; }

        public bool NeedTwoRegisters { get; set; }

        public Register(XElement reg)
        {
            Path = string.Join(PathDel.ToString(), reg.Element("Path").Elements().Select(a => a.Value));
            SelectedDataType = reg.Element("DataType").Value;
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
