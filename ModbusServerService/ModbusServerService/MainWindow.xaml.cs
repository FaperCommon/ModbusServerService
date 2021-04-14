using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public partial class MainWindow : Window
    {

        public string Filepath { get; set; } = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";//
        ConfigViewModel _regVM;
        public MainWindow()
        {
            InitializeComponent();
            _regVM = new ConfigViewModel();
            DataContext = _regVM;
            string dir = @"C:\INTMABW500MBTCPService";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(Filepath))
            {
                _regVM.UpdateConfig(Filepath);
            }
            _regVM.ConfingRead(Filepath);
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
 //
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try {
                _regVM.UpdateConfig(Filepath);
                MessageBox.Show("Конфигурация успешно обновлена!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddReg_Click(object sender, RoutedEventArgs e)
        {
            var wA = new AddWebSourceWindow();
            wA.ShowDialog();
            if (!String.IsNullOrEmpty(wA.AddedWebSource.WebAddress))
            {
                _regVM.WebSources.Add(wA.AddedWebSource);
            }
        }
    }
}
