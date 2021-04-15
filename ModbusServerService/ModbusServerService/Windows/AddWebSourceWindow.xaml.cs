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
        }
        public AddWebSourceWindow(WebSourceViewModel addedWebSource)
        {
            InitializeComponent();
            AddedWebSource = addedWebSource;
            tbDur.Text = AddedWebSource.Duration.ToString();
            tbAddress.Text = AddedWebSource.WebAddress;
            tbName.Text = AddedWebSource.Name;
            onEdit = true;
            btnAccept.Content = "Принять";
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
            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Поле с именем должно быть заполнено!");
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
            AddedWebSource.Name = tbName.Text;
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
