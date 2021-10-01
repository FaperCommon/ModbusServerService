using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public class RegistersGroupVM : TreeViewItem
    {
        RegistersGroup _registersGroup;
        RegisterVM _selectedRegister;
        override public string Name { get => _registersGroup.Name; set { _registersGroup.Name = value; OnPropertyChanged(); } }
        public ObservableCollection<RegisterVM> RegistersVM { get; }
        public RegistersGroupVM(RegistersGroup registersGroup)
        {
            Actions = new ObservableCollection<ContextAction>() {
                new ContextAction() { Name = "Добавить регистр", Action = AddRegisterCommand },
                new ContextAction() { Name = "Дублировать", Action = DublicateNodeCommand },
                new ContextAction() { Name = "Удалить", Action = DeleteNodeCommand },
                new ContextAction() { Name = "Свойства", Action = PropertyShowCommand }
            };

            _registersGroup = registersGroup;
            RegistersVM = new ObservableCollection<RegisterVM>();

            foreach (var reg in registersGroup.Registers)
            {
                RegistersVM.Add(new RegisterVM(reg));
            }
        }

        public RegistersGroupVM()
        {
        }

        public RegisterVM SelectedRegister
        {
            get { return _selectedRegister; }
            set
            {
                _selectedRegister = value;
                OnPropertyChanged();
            }
        }

        private bool CanOperatWithRegister
        {
            get { return SelectedRegister != null; }
        }

        private void DeleteSelectedRegister()
        {
            _registersGroup.Registers.Remove(_registersGroup.Registers.FirstOrDefault(a => (object)a.Path == (object)SelectedRegister.Path));
            RegistersVM.Remove(SelectedRegister);
        }

        private void DublicateSelectedRegister()
        {
            RegistersVM.Insert(RegistersVM.IndexOf(SelectedRegister), new RegisterVM((Register)SelectedRegister.Clone()));
        }

        private ICommand _dublicateRegisterCommand;
        public ICommand DublicateRegisterCommand
        {
            get
            {
                if (_dublicateRegisterCommand == null)
                {
                    _dublicateRegisterCommand = new RelayCommand(param => DublicateSelectedRegister(), param => CanOperatWithRegister);
                }
                return _dublicateRegisterCommand;
            }
        }

        private ICommand _deleteRegisterCommand;
        public ICommand DeleteRegisterCommand
        {
            get
            {
                if (_deleteRegisterCommand == null)
                {
                    _deleteRegisterCommand = new RelayCommand(param => DeleteSelectedRegister(), param => CanOperatWithRegister);
                }
                return _deleteRegisterCommand;
            }
        }

        override protected void PropertyShow()
        {
            var wA = new Windows.AddRegGroupWindow(this);
            wA.ShowDialog();
            OnPropertyChanged("Name");
        }

        private void AddRegister()//
        {
            var reg = new Register();
            var regVM = new RegisterVM(reg);
            var wA = new Windows.AddRegisterWindow(regVM);
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                if (_registersGroup.Registers.Any(a => a.Path == reg.Path))
                {
                    MessageBox.Show("Путь уже добавлен!");
                    return;
                }

                RegistersVM.Add(regVM);
                _registersGroup.Registers.Add(reg);
            }
        }

        private ICommand _addRegisterCommand;
        public ICommand AddRegisterCommand
        {
            get
            {
                if (_addRegisterCommand == null)
                {
                    _addRegisterCommand = new RelayCommand(param => AddRegister(), param => true);
                }
                return _addRegisterCommand;
            }
        }

        override public object Clone()
        {
            return new RegistersGroupVM((RegistersGroup)_registersGroup.Clone());
        }
    }
}
