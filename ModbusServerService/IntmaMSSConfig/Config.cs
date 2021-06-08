using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class Config
    {
        public int Duration { get; set; }
        public string Port { get; set; }
        public string ModbusServerAddress { get; set; }
        public ObservableCollection<WebSource> WebSources { get; set; }
        public string Filepath { get; set; } = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
        public Config()
        {
            WebSources = new ObservableCollection<WebSource>();
            string dir = @"C:\INTMABW500MBTCPService";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(Filepath))
            {
                Port = "502";
                ModbusServerAddress = "0.0.0.0";
                Duration = 5;
                UpdateConfig();
            }
            ConfingRead();
        }
        
        public void ConfingRead()
        {
            var doc = XDocument.Load(Filepath);
            var el = doc.Element("Config");
            ModbusServerAddress = el.Element("ModbusServerAddress").Value;
            Port = el.Element("Port").Value;
            Duration = Int32.Parse(el.Element("Duration").Value);
            WebSources = new ObservableCollection<WebSource>();
            foreach (var source in el.Element("Sources").Elements())
            {
                WebSources.Add(new WebSource(source));
            }
        }

        public void UpdateConfig()
        {
            XElement contacts =
                new XElement("Config",
                    new XElement("Port", $"{Port}"),
                    new XElement("Duration", $"{Duration}"),
                    new XElement("ModbusServerAddress", $"{ModbusServerAddress}"),
                    new XElement("Sources",
                        WebSources.Select((source, i) => new XElement($"Source{i}",
                            new XElement("Duration", source.Duration),
                            new XElement("Name", source.Name),
                            new XElement("WebAddress", source.WebAddress),
                            new XElement("Weights", source.RegistersGroups.Select((weight, j) => new XElement($"Weight{j}",
                                new XElement("Name", weight.Name),
                                new XElement("Registers", weight.Registers.Select((reg, k) => new XElement($"Reg{k}",
                                    new XElement("NeedTwoRegisters", reg.NeedTwoRegisters),
                                    new XElement("ValueRegister", reg.ValueRegister),
                                    new XElement("DataType", reg.DataType),
                                    new XElement("Path",
                                    reg.Path.Split(Register.PathDel).Select(path => new XElement(path, path)))))))))))));

            XDocument s = new XDocument(contacts);
            s.Save(Filepath);
        }

    }
}
