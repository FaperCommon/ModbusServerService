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

namespace Intma.ModbusServerService.Configurator
{
    public partial class AddWebSourceWindow : Window
    {
        public WebSourceViewModel AddedWebSource { get; }
        public AddWebSourceWindow()
        {
            InitializeComponent();
            AddedWebSource = new WebSourceViewModel();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbDur.Text))
            {
                MessageBox.Show("Поле с частотой опроса должно быть заполнено!");
                return;
            }
            if (String.IsNullOrEmpty(tbAddress.Text))
            {
                MessageBox.Show("Поле с адресом должно быть заполнено!");
                return;
            }
            int value;
            if (int.TryParse(tbDur.Text, out value))
                AddedWebSource.Duration = value;
            else
            {
                MessageBox.Show("Не удалось распознать номер регистра");
                return;
            }
            AddedWebSource.WebAddress = tbAddress.Text;
            MessageBox.Show("Запись успешно добавлена!");
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
