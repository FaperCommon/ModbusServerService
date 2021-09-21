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
        public WebSourceVM AddedWebSource { get; private set; }
        public bool IsAdded { get; set; } 

        private bool onEdit;

        public AddWebSourceWindow(WebSourceVM addedWebSource)
        {
            InitializeComponent();
            AddedWebSource = addedWebSource;
            DataContext = (WebSourceVM)addedWebSource.Clone();
            onEdit = true;
            btnAccept.Content = "Принять";
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var source = (WebSourceVM)DataContext;
            if (String.IsNullOrEmpty(source.WebAddress.Replace(@"http:\\", "")))
            {
                MessageBox.Show("Поле с адресом должно быть заполнено!");
                return;
            }

            if (source.Duration <= 0)
            {
                MessageBox.Show("Неверно указана частота");
                return;
            }

            if (onEdit) { 
                AddedWebSource.Duration = source.Duration;
                AddedWebSource.WebAddress = source.WebAddress;
            }

            IsAdded = true;
            Close();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
