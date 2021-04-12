using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace Intma.ModbusServerService.Configurator
{
    public partial class AddRegisterWindow : Window
    {
        public Register AddedRegister;
        public AddRegisterWindow()
        {

            InitializeComponent();

            // ObservableCollection<string> types = new ObservableCollection<string>() {"Word","Float","Int"};

            AddedRegister = new Register();
            cmbType.ItemsSource = AddedRegister.DataType;
            //cmbType.SelectedItem = AddedRegister.SelectedDataType;
            //cmbType.SelectedIndex = 0;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbReg1.Text))
            {
                MessageBox.Show("Поле с номером регистра должно быть заполнено");
                return;
            }
            if (String.IsNullOrEmpty(tbPath.Text))
            {
                MessageBox.Show("Поле с номером регистра должно быть заполнено");
                return;
            }

            int value;
            if (int.TryParse(tbReg1.Text, out value))
                AddedRegister.ValueRegister = value;
            else
            {
                MessageBox.Show("Не удалось распознать регистр");
                return;
            }
            //AddedRegister.SelectedDataType = type;
            AddedRegister.SelectedDataType = cmbType.SelectedItem.ToString();
            AddedRegister.Path = tbPath.Text;
            MessageBox.Show("Запись успешно добавлена!");
            Close();
        }
        
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CmbReg1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
