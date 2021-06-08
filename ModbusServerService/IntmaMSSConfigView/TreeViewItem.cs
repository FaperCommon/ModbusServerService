using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public abstract class TreeViewItem : Notify
    {
        public bool NeedDublicate { get; set; }
        public bool NeedDelete { get; set; }
        abstract public string Name { get; set; }
        public BindingList<TreeViewItem> Childs { get; set; }

        public ObservableCollection<ContextAction> Actions { get; set; }

        private ICommand _dublicateNodeCommand;
        public ICommand DublicateNodeCommand
        {
            get
            {
                if (_dublicateNodeCommand == null)
                {
                    _dublicateNodeCommand = new RelayCommand(param => { NeedDublicate = true; OnPropertyChanged("NeedDublicate"); }, param => true);
                }
                return _dublicateNodeCommand;
            }
        }

        private ICommand _deleteNodeCommand;
        public ICommand DeleteNodeCommand
        {
            get
            {
                if (_deleteNodeCommand == null)
                {
                    _deleteNodeCommand = new RelayCommand(param => { NeedDelete = true; OnPropertyChanged("NeedDelete"); }, param => true);
                }
                return _deleteNodeCommand;
            }
        }

        private ICommand _PropertyShowCommand;
        public ICommand PropertyShowCommand
        {
            get
            {
                if (_PropertyShowCommand == null)
                {
                    _PropertyShowCommand = new RelayCommand(param => PropertyShow(), param => true);
                }
                return _PropertyShowCommand;
            }
        }

        private ICommand _addChildCommand;
        public ICommand AddChildCommand
        {
            get
            {
                if (_addChildCommand == null)
                {
                    _addChildCommand = new RelayCommand(param => AddChild(), param => true);
                }
                return _addChildCommand;
            }
        }

        virtual protected void AddChild() { }
        abstract protected void PropertyShow();
        abstract public object Clone();

        public bool _isSelected;
        public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(); } }
    }
}
