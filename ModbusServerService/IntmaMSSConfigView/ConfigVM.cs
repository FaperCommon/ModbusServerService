﻿using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Intma.ModbusServerService.Configurator
{
    public class ConfigVM : Notify
    {
        private Config _config;
        private WebSourceVM _selectedSource;
        private RegistersGroupVM _selectedGroup;

        public string ModbusServerAddress => _config.ModbusServerAddress;
        public int Duration { get => _config.Duration; set => _config.Duration = value; }
        public string Port => _config.Port; //WHAT
        public BindingList<WebSourceVM> Childs { get; set; }
        public WebSourceVM SelectedSource { get => _selectedSource; set { _selectedSource = value; OnPropertyChanged(); } }
        public RegistersGroupVM SelectedGroup { get => _selectedGroup; set { _selectedGroup = value; OnPropertyChanged(); } }
        
        public ConfigVM()
        {
            _config = new Config();
            Childs = new BindingList<WebSourceVM>();
            Childs.ListChanged += Childs_ListChanged;
            foreach (var source in _config.WebSources)
            {
                Childs.Add(new WebSourceVM(source));
            }
        }

        private void Childs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.NewIndex < Childs.Count && Childs.Count > 0)
            {
                if (Childs[e.NewIndex].NeedDelete)
                {
                    Childs.Remove(Childs[e.NewIndex]);
                }
                else if (Childs[e.NewIndex].NeedDublicate)
                {
                    Childs[e.NewIndex].NeedDublicate = false;
                    Childs.Insert(e.NewIndex + 1, (WebSourceVM)Childs[e.NewIndex].Clone());
                }
                else { 
                    SelectedSource = Childs[e.NewIndex];
                    SelectedGroup = Childs[e.NewIndex].SelectedGroup;
                }
            }
            else if (Childs.Count == 0)
            {
                SelectedGroup = null;
            }
        }

        private void AcceptConfig()
        {
            try
            {
                _config.UpdateConfig();
                MessageBox.Show("Конфигурация успешно обновлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ICommand _acceptConfigCommand;
        public ICommand AcceptConfigCommand
        {
            get
            {
                if (_acceptConfigCommand == null)
                {
                    _acceptConfigCommand = new RelayCommand(param => AcceptConfig(), param => true);
                }
                return _acceptConfigCommand;
            }
        }

        private ICommand _addWebSourceCommand;
        public ICommand AddWebSourceCommand
        {
            get
            {
                if (_addWebSourceCommand == null)
                {
                    _addWebSourceCommand = new RelayCommand(param => AddWebSource(), param => true);
                }
                return _addWebSourceCommand;
            }
        }

        private void AddWebSource()
        {
            var source = new WebSource();
            var sourceVM = new WebSourceVM(source);

            var wA = new Windows.AddWebSourceWindow(sourceVM);
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                _config.WebSources.Add(source);
                Childs.Add(wA.AddedWebSource);
            }
        }

        private void AutoRegisters()    //Check
        {
            int i = 1;
            foreach (var source in Childs)
                foreach (RegistersGroupVM group in source.Childs)
                    foreach (var reg in group.RegistersVM)
                    {
                        reg.ValueRegister = i++;
                        if (reg.DataType == "Float")
                            i++;
                    }
        }

        private ICommand _autoRegistersCommand;
        public ICommand AutoRegistersCommand
        {
            get
            {
                if (_autoRegistersCommand == null)
                {
                    _autoRegistersCommand = new RelayCommand(param => AutoRegisters(), param => true);
                }
                return _autoRegistersCommand;
            }
        }
    }
}
