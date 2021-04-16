using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class RegistersGroupViewModel : Notify, ICloneable
    {
        string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        ObservableCollection<Register> _registers;
        public string Type { get; } = "RegistersGroupViewModel";

        public RegistersGroupViewModel(XElement source)
        {
            Name = source.Element("Name").Value;
            Registers = new ObservableCollection<Register>();
            foreach (var reg in source.Element("Registers").Elements())
            {
                Registers.Add(new Register(reg));
            }
        }
        public RegistersGroupViewModel()
        {
            Registers = new ObservableCollection<Register>();
        }
        public ObservableCollection<Register> Registers { get => _registers; set { _registers = value; OnPropertyChanged(); } }
        Register _selectedRegister;
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set
            {
                _selectedRegister = value;
                OnPropertyChanged();
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
        private void DublicateSelected()
        {
            Registers.Insert(Registers.IndexOf(SelectedRegister) + 1,(Register)SelectedRegister.Clone());
        }

        public object Clone()
        {
            var clone = (RegistersGroupViewModel)this.MemberwiseClone();
            clone.Registers = new ObservableCollection<Register>();
            foreach(var reg in Registers)
            {
                clone.Registers.Add((Register)reg.Clone());
            }
            clone.SelectedRegister = null;
            clone._dublicateCommand = null;
            return clone;
        }
        private ICommand _dublicateCommand;
        public ICommand DublicateCommand
        {
            get
            {
                if (_dublicateCommand == null)
                {
                    _dublicateCommand = new RelayCommand(param => DublicateSelected(), param => CanOperate);
                }
                return _dublicateCommand;
            }
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
                sb.Append($"{reg.ToString(Name)},{reg.SelectedDataType},1,R/W,1000,,,,,,,,,,\"\",\n");
            return sb.ToString();
        }
    }
}
