﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Intma.ModbusServerService.Configurator.Windows
{
    public partial class AddRegisterWindow : Window
    {
        public Register AddedRegister { get; }
        public bool IsAdded { get; set; } = false;
        public AddRegisterWindow()
        {
            InitializeComponent();
            AddedRegister = new Register();
            DataContext = AddedRegister;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddedRegister.ValueRegister < 0)
            {
                MessageBox.Show("Поле с номером регистра должно быть заполнено");
                return;
            }
            if (String.IsNullOrEmpty(AddedRegister.Path))
            {
                MessageBox.Show("Поле с путем должно быть заполнено");
                return;
            }
            if (String.IsNullOrEmpty(AddedRegister.SelectedDataType))
            {
                MessageBox.Show("Тип данных не выбран!");
                return;
            }
           
            MessageBox.Show("Запись успешно добавлена!");
            IsAdded = true;
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
