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
            if (treeView1.SelectedItem is RegistersGroupViewModel)
            {
                var item = (RegistersGroupViewModel)treeView1.SelectedItem;
                groupGrid.DataContext = item;
            }
            else
            {
                groupGrid.DataContext = null;
            }
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

        private void WebSourceAddWindow_Click(object sender, RoutedEventArgs e)
        {
            var wA = new Windows.AddWebSourceWindow();
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                _regVM.Childs.Add(wA.AddedWebSource);
            }
        }
   
        private void RegistersGroupAddWindow_Click(object sender, RoutedEventArgs e)
        {
            var wA = new Windows.AddRegGroupWindow();
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                ((WebSourceViewModel)treeView1.SelectedItem).Childs.Add(wA.AddedRegistersGroup);
            }
        }

        private void RegistersAddWindow_Click(object sender, RoutedEventArgs e)
        {
            if(groupGrid.DataContext is RegistersGroupViewModel) { 
                var wA = new Windows.AddRegisterWindow();
                wA.ShowDialog();
                if (wA.IsAdded)
                {
                    ((RegistersGroupViewModel)groupGrid.DataContext).Registers.Add(wA.AddedRegister);
                }
            }
        }

        private void WebSourcePropertyWindow_Click(object sender, RoutedEventArgs e)
        {

            var wA = new Windows.AddWebSourceWindow((WebSourceViewModel)treeView1.SelectedItem);
            wA.ShowDialog();
        }

        private void GroupPropertyWindow_Click(object sender, RoutedEventArgs e)
        {
            var wA = new Windows.AddRegGroupWindow((RegistersGroupViewModel)treeView1.SelectedItem);
            wA.ShowDialog();
        }

        private void WebSourceDeleteWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить источник?", "Подтвердите действие", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _regVM.Childs.Remove((WebSourceViewModel)treeView1.SelectedItem);
            }
        }

        private void GroupDeleteWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить группу?", "Подтвердите действие", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var removed = (RegistersGroupViewModel)treeView1.SelectedItem;
                _regVM.Childs.First(a => a.Childs.Contains(removed)).Childs.Remove(removed);
            }
        }

        private void ExportForKepServer_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv) | *.csv";
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == true) { 
                File.WriteAllText(saveFileDialog.FileName, ((WebSourceViewModel)treeView1.SelectedItem).ExportTopicForKepServer());
            }
        }

        private void ExportTopicForWonderware_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv) | *.csv";
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == true) { 
                File.WriteAllText(saveFileDialog.FileName, ((WebSourceViewModel)treeView1.SelectedItem).ExportTopicForWonderware());
            }
        }

        private void GroupDublicateWindow_Click(object sender, RoutedEventArgs e)
        {
            var orig = ((RegistersGroupViewModel)treeView1.SelectedItem);
            var source =  _regVM.Childs.First(a => a.Childs.Contains(orig));
            var clone = (RegistersGroupViewModel)orig.Clone();
            source.Childs.Insert(source.Childs.IndexOf(orig) + 1, (RegistersGroupViewModel)clone.Clone());
        }

        private void WebSourceDublicateWindow_Click(object sender, RoutedEventArgs e)
        {
            var source = ((WebSourceViewModel)treeView1.SelectedItem);
            var clone = (WebSourceViewModel)source.Clone();
            _regVM.Childs.Insert(_regVM.Childs.IndexOf(source) + 1, (WebSourceViewModel)clone.Clone());
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
