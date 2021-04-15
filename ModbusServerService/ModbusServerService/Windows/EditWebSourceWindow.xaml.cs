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
    /// Логика взаимодействия для EditWebSourceWindow.xaml
    /// </summary>
    public partial class EditWebSourceWindow : UserControl
    {
        WebSourceViewModel _wsVM;
        public EditWebSourceWindow(WebSourceViewModel webSource)
        {
            InitializeComponent();
            _wsVM = webSource;
            DataContext = _wsVM;
        }

        private void AddReg_Click(object sender, RoutedEventArgs e)
        {
            var wA = new AddRegisterWindow();
            wA.ShowDialog();
            if (!String.IsNullOrEmpty(wA.AddedRegister.Path))
            {
                //_wsVM.Childs.Add(wA.AddedRegister);
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Конфигурация успешно обновлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
