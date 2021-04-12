using EasyModbus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public partial class MainWindow : Window
    {

        public string Filepath { get; set; } = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
        public RegistersViewModel RegVM;
        public MainWindow()
        {
            InitializeComponent();
            RegVM = new RegistersViewModel();
            DataContext = RegVM;
            string dir = @"C:\INTMABW500MBTCPService";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(Filepath))
            {
                UpdateConfig(Filepath,"10.10.10.10", "502" ,5,"127.0.0.1", new List<Register>(new Register[] { new Register() { Path = "Dx,D0,p0", SelectedDataType = "Word", ValueRegister = 0 } }));
            }
            ConfingRead(Filepath);
        }

    
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try {
                UpdateConfig(Filepath, RegVM.WebAdress, RegVM.Port, RegVM.Duration, RegVM.ModbusServerAdress, RegVM.Registers);
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
            RegVM.WebAdress = el.Element("WebAdress").Value;
            RegVM.ModbusServerAdress = el.Element("ModbusServerAdress").Value;
            RegVM.Port = el.Element("Port").Value;
            RegVM.Duration = Int32.Parse(el.Element("Duration").Value);
            RegVM.Registers = new ObservableCollection<Register>();
            foreach (var reg in el.Element("Registers").Elements())
            {
                RegVM.Registers.Add(new Register(reg));
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
                            new XElement("DataType", a.SelectedDataType), 
                            new XElement("Path", 
                                a.Path.Split(a.PathDel).Select(path => new XElement(path,path)))))));
            XDocument s = new XDocument(contacts);
            s.Save(filepath);
        }

        private void AddReg_Click(object sender, RoutedEventArgs e)
        {
            var wA = new AddRegisterWindow();
            wA.ShowDialog();
            if(!String.IsNullOrEmpty(wA.AddedRegister.Path))
            {
                RegVM.Registers.Add(wA.AddedRegister);
            }
        }
    }
}
