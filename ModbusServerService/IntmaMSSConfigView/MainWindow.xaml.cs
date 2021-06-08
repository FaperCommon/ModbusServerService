using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Intma.ModbusServerService.Configurator
{
    public partial class MainWindow : Window
    {
        ConfigVM _confVM;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _confVM = new ConfigVM();
                DataContext = _confVM;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
