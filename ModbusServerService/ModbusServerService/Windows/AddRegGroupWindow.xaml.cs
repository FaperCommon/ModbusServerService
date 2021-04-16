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
            DataContext = AddedRegistersGroup;
        }
        public AddRegGroupWindow(RegistersGroupViewModel addedRegistersGroup)
        {
            InitializeComponent();
            AddedRegistersGroup = addedRegistersGroup;
            DataContext = (RegistersGroupViewModel)addedRegistersGroup.Clone();
            onEdit = true;
            btnAccept.Content = "Принять";
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var group = (RegistersGroupViewModel)DataContext;
            if (String.IsNullOrEmpty(group.Name))
            {
                MessageBox.Show("Поле с именем должно быть заполнено!");
                return;
            }
            if (onEdit) { 
                AddedRegistersGroup.Name = group.Name;
            }

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
