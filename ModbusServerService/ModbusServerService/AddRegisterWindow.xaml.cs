using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Intma.ModbusServerService.Configurator
{
    public partial class AddRegisterWindow : Window
    {
        public Register AddedRegister { get; }
        public AddRegisterWindow()
        {
            InitializeComponent();
            AddedRegister = new Register();
            cmbType.ItemsSource = AddedRegister.DataType;
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
                MessageBox.Show("Не удалось распознать номер регистра");
                return;
            }
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
            if (AddedRegister.NeedTwoRegisters)
            {
                int value;
                //if (int.TryParse(tbReg1.Text, out value))
                //    AddedRegister.ValueRegister = value;
                //tbReg2.Text = (value + 1).ToString();////////
            }
        }
    }
}
