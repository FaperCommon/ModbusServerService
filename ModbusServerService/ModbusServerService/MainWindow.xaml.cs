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

//refactor, recode to MVVM
//

namespace Intma.ModbusServerService.Configurator
{
    public partial class MainWindow : Window
    {
        
        public string Filepath { get; set; } = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
        Config _regVM;
        public MainWindow()
        {
            InitializeComponent();
            _regVM = new Config();
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
            if (treeView1.SelectedItem is RegistersGroup)
            {
                var item = (RegistersGroup)treeView1.SelectedItem;
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

        private void AutoRegisters_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            foreach (var source in _regVM.Childs)
                foreach (var group in source.Childs)
                    foreach (var reg in group.Registers)
                    {
                        reg.ValueRegister = i++;
                        if (reg.NeedTwoRegisters)
                            i++;
                    }
        }

        private void RegistersGroupAddWindow_Click(object sender, RoutedEventArgs e)
        {
            var wA = new Windows.AddRegGroupWindow();
            wA.ShowDialog();
            if (wA.IsAdded)
            {
                ((WebSource)treeView1.SelectedItem).Childs.Add(wA.AddedRegistersGroup);
            }
        }

        private void RegistersAddWindow_Click(object sender, RoutedEventArgs e)
        {
            if(groupGrid.DataContext is RegistersGroup) { 
                var wA = new Windows.AddRegisterWindow();
                wA.ShowDialog();
                if (wA.IsAdded)
                {
                    ((RegistersGroup)groupGrid.DataContext).Registers.Add(wA.AddedRegister);
                }
            }
        }

        private void WebSourcePropertyWindow_Click(object sender, RoutedEventArgs e)
        {

            var wA = new Windows.AddWebSourceWindow((WebSource)treeView1.SelectedItem);
            wA.ShowDialog();
        }

        private void GroupPropertyWindow_Click(object sender, RoutedEventArgs e)
        {
            var wA = new Windows.AddRegGroupWindow((RegistersGroup)treeView1.SelectedItem);
            wA.ShowDialog();
        }

        private void WebSourceDeleteWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить источник?", "Подтвердите действие", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _regVM.Childs.Remove((WebSource)treeView1.SelectedItem);
            }
        }

        private void GroupDeleteWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить группу?", "Подтвердите действие", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var removed = (RegistersGroup)treeView1.SelectedItem;
                _regVM.Childs.First(a => a.Childs.Contains(removed)).Childs.Remove(removed);
            }
        }

        private void ExportForKepServer_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv) | *.csv";
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == true) { 
                File.WriteAllText(saveFileDialog.FileName, ((WebSource)treeView1.SelectedItem).ExportTopicForKepServer());
            }
        }

        private void ExportTopicForWonderware_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv) | *.csv";
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == true) { 
                File.WriteAllText(saveFileDialog.FileName, ((WebSource)treeView1.SelectedItem).ExportTopicForWonderware());
            }
        }

        private void GroupDublicateWindow_Click(object sender, RoutedEventArgs e)
        {
            var orig = ((RegistersGroup)treeView1.SelectedItem);
            var source =  _regVM.Childs.First(a => a.Childs.Contains(orig));
            var clone = (RegistersGroup)orig.Clone();
            source.Childs.Insert(source.Childs.IndexOf(orig) + 1, (RegistersGroup)clone.Clone());
        }

        private void WebSourceDublicateWindow_Click(object sender, RoutedEventArgs e)
        {
            var source = ((WebSource)treeView1.SelectedItem);
            var clone = (WebSource)source.Clone();
            _regVM.Childs.Insert(_regVM.Childs.IndexOf(source) + 1, (WebSource)clone.Clone());
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
