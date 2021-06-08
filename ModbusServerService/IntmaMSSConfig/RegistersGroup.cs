using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class RegistersGroup : ICloneable
    {
        public string Name { get; set; }
        public ObservableCollection<Register> Registers { get; set; }

        public RegistersGroup(XElement source)
        {
            Name = source.Element("Name").Value;
            Registers = new ObservableCollection<Register>();
            foreach (var reg in source.Element("Registers").Elements())
            {
                Registers.Add(new Register(reg));
            }
        }

        public RegistersGroup()
        {
            Registers = new ObservableCollection<Register>();
        }

        public object Clone()
        {
            var clone = (RegistersGroup)this.MemberwiseClone();
            clone.Registers = new ObservableCollection<Register>();
            foreach(var reg in Registers)
            {
                clone.Registers.Add((Register)reg.Clone());
            }
         //   clone.SelectedRegister = null;
         //   clone._dublicateCommand = null;
            return clone;
        }
       
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var reg in Registers)
                sb.AppendFormat("{0}\n", reg.ToString(Name));
            return sb.ToString();
        }

        public string ToStringForKepServer()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var reg in Registers)
                sb.Append($"{reg.ToString(Name)},{reg.DataType},1,R/W,1000,,,,,,,,,,\"\",\n");
            return sb.ToString();
        }
    }
}
