using EasyModbus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string _webAdress;
        public string _port;
        public string _modbusServerAdress;
        public int _duration;
        public ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers { get => _registers; set { _registers = value; OnPropertyChanged(); } }
        public string WebAdress { get => _webAdress; set { _webAdress = value; OnPropertyChanged(); } }
        public string Port { get => _port; set { _port = value; OnPropertyChanged(); } }
        public string ModbusServerAdress { get => _modbusServerAdress; set { _modbusServerAdress = value; OnPropertyChanged(); } }
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(); } }

        public string Filepath { get; set; } = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
        public MainWindow()
        {
            InitializeComponent();
            Registers = new ObservableCollection<Register>();
            DataContext = this;
            string dir = @"C:\INTMABW500MBTCPService";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(Filepath))
            {
                UpdateConfig(Filepath,"10.10.10.10", "502" ,5,"127.0.0.1", new List<Register>(new Register[] { new Register() { Path = "Dx,D0,p0", DataType = "Word", ValueRegister = 0 } }));
            }
            ConfingRead(Filepath);
        }

    
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try {
                UpdateConfig(Filepath, WebAdress, Port,Duration,ModbusServerAdress, Registers);
                MessageBox.Show("Конфигурация успешно обновлена!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ConfingRead(string filepath)
        {
            var doc = XDocument.Load(filepath);
            var el = doc.Element("config");
            WebAdress = el.Element("WebAdress").Value;
            ModbusServerAdress = el.Element("ModbusServerAdress").Value;
            Port = el.Element("Port").Value;
            Duration = Int32.Parse(el.Element("Duration").Value);
            Registers = new ObservableCollection<Register>();
            foreach (var reg in el.Element("Registers").Elements())
            {
                Registers.Add(new Register(reg));
            }
        }


        public void UpdateConfig(string filepath, string webAdress, string port, int duration, string modbusServerAdress, IEnumerable<Register> registers)
        {
            int regNumber = 1;
            XElement contacts =
                new XElement("config",
                    new XElement("WebAdress", $"{webAdress}"),
                    new XElement("Port", $"{port}"),
                    new XElement("Duration", $"{duration}"),
                    new XElement("ModbusServerAdress", $"{modbusServerAdress}"),
                    new XElement("Registers", 
                        registers.Select(a => new XElement($"Reg{regNumber++}", 
                            new XElement("IsFloat", a.IsFloat), 
                            new XElement("Value", a.Value),
                            new XElement("Mantissa", a.Mantissa),
                            new XElement("MantissaRegister", a.MantissaRegister),
                            new XElement("ValueRegister", a.ValueRegister),
                            new XElement("DataType", a.DataType), 
                            new XElement("Path", 
                                a.Path.Split(a.PathDel).Select(path => new XElement(path,path)))))));
            XDocument s = new XDocument(contacts);
            s.Save(filepath);
        }

        private void AddReg_Click(object sender, RoutedEventArgs e)
        {
            var wA = new AddRegisterWindow();
            wA.ShowDialog();
            if(wA.AddedRegister != null)
            {
                Registers.Add(wA.AddedRegister);
            }
        }

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
