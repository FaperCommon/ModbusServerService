using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Intma.ModbusServerService.Configurator.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddRegGroupWindow.xaml
    /// </summary>
    public partial class AddRegGroupWindow : Window
    {
        public RegistersGroupViewModel AddedRegistersGroup { get; }
        public bool IsAdded { get; set; } = false;
        public bool onEdit;
        public AddRegGroupWindow()
        {
            InitializeComponent();
            AddedRegistersGroup = new RegistersGroupViewModel();
        }
        public AddRegGroupWindow(RegistersGroupViewModel addedRegistersGroup)
        {
            InitializeComponent();
            AddedRegistersGroup = addedRegistersGroup;
            tbName.Text = AddedRegistersGroup.Name;
            onEdit = true;
            btnAccept.Content = "Принять";
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Поле с именем должно быть заполнено!");
                return;
            }
            
            AddedRegistersGroup.Name = tbName.Text;
            MessageBox.Show(onEdit ? "Запись успешно редактирована" : "Запись успешно добавлена!");
            IsAdded = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
