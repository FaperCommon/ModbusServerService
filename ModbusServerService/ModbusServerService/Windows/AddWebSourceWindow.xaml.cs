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
    public partial class AddWebSourceWindow : Window
    {
        public WebSourceViewModel AddedWebSource { get; }
        public bool IsAdded { get; set; } = false;

        private bool onEdit;
        public AddWebSourceWindow()
        {
            InitializeComponent();
            AddedWebSource = new WebSourceViewModel();
            DataContext = AddedWebSource;
        }
        public AddWebSourceWindow(WebSourceViewModel addedWebSource)
        {
            InitializeComponent();
            AddedWebSource = addedWebSource;
            DataContext = (WebSourceViewModel)addedWebSource.Clone();
            onEdit = true;
            btnAccept.Content = "Принять";
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var source = (WebSourceViewModel)DataContext;
            if (String.IsNullOrEmpty(source.WebAddress.Replace(@"http:\\", "")))
            {
                MessageBox.Show("Поле с адресом должно быть заполнено!");
                return;
            }
            if (String.IsNullOrEmpty(source.Name))
            {
                MessageBox.Show("Поле с именем должно быть заполнено!");
                return;
            }
            if (source.Duration <= 0)
            {
                MessageBox.Show("Неверно указана частота");
                return;
            }
            if (onEdit) { 
                AddedWebSource.Duration = source.Duration;
                AddedWebSource.Name = source.Name;
                AddedWebSource.WebAddress = source.WebAddress;
            }
            MessageBox.Show(onEdit?"Запись успешно редактирована":"Запись успешно добавлена!");
            IsAdded = true;
            Close();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
