using EasyModbus;
using Intma.ModbusServerService.Configurator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Intma.ModbusServerService
{
    class HttpXmlReader : IDisposable
    {
        ConfigViewModel config;

        ModbusServer _modbusServer;
        System.Diagnostics.EventLog _eventLog;
        string _configFilePath = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
        string _logFilePath = @"C:\INTMABW500MBTCPService\IntmaModbusServerService.log";

        public HttpXmlReader()
        {
            _modbusServer = new ModbusServer();
            config = new ConfigViewModel();
            if (!System.Diagnostics.EventLog.Exists("IntmaModbusServerService EventLog"))
                System.Diagnostics.EventLog.CreateEventSource("IntmaModbusServerService", "IntmaModbusServerService EventLog");

            _eventLog = new System.Diagnostics.EventLog("IntmaModbusServerService EventLog");
            _eventLog.Source = "IntmaModbusServerService";

            ReConfigur();
            StartServer();
        }

        public void GetValue(WebSourceViewModel webSource)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webSource.WebAddress);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    StreamReader readStream = null;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream receiveStream = response.GetResponseStream())
                        {
                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                            }
                            foreach (var el in XmlParse(readStream, webSource.Childs))
                                foreach (var reg in el.Registers)
                                    WriteValue(reg, el.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Web read ex {webSource.WebAddress}: " + ex.Message);
                _eventLog.WriteEntry($"Web Read ex {webSource.WebAddress}: " + ex.Message);
            }
        }

        public IList<RegistersGroupViewModel> XmlParse(StreamReader streamReader, IList<RegistersGroupViewModel> registersGroups)
        {
            try
            {
                XDocument doc;
                doc = XDocument.Load(streamReader, LoadOptions.None);

                foreach (var regGroup in registersGroups)
                {
                    for (int i = 0; i < regGroup.Registers.Count; i++)
                    {
                        var arr = regGroup.Registers[i].Path.Split(Register.PathDel);
                        var el = doc.Element(arr[0]);
                        for (int j = 1; j < arr.Length; j++)
                        {
                            el = el.Element(arr[j]);
                        }

                        regGroup.Registers[i].Value = el.Value;
                    }
                }

                if (streamReader != null)
                    streamReader.Close();

                return registersGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xml Parse ex: " + ex.Message);
                _eventLog.WriteEntry("Xml Parse ex: " + ex.Message);
                return null;
            }
        }

        public void StartServer()
        {
            try
            {
                //_modbusServer.LocalIPAddress = IPAddress.Parse(ModbusServerAdress);
                _modbusServer.LogFileFilename = _logFilePath;
                _modbusServer.HoldingRegistersChanged += ModbusServer_HRChanged;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                _modbusServer.Listen();
                Console.WriteLine(_modbusServer.LocalIPAddress + ", Server started!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start server ex: " + ex.Message);
                _eventLog.WriteEntry("Start server ex: " + ex.Message);
            }
        }

        private void ModbusServer_HRChanged(int hr, int numberOfHR)
        {
            try
            {
                Console.WriteLine("ModbusServer_HRChanged");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _eventLog.WriteEntry("ModbusServer_HRChanged ex:" + ex.Message);
            }
        }

        void WriteValue(Intma.ModbusServerService.Configurator.Register reg, string groupName)
        {
            if (String.IsNullOrEmpty(reg.Value.ToString()))
                return;
            try
            {
                byte[] buff4b;
                if (reg.NeedTwoRegisters)
                {
                    if (reg.SelectedDataType == "Float")
                    {
                        buff4b = BitConverter.GetBytes(Single.Parse(reg.Value.ToString()));
                    }
                    else
                    {
                        buff4b = BitConverter.GetBytes(Int32.Parse(reg.Value.ToString()));
                    }
                }
                else
                    buff4b = BitConverter.GetBytes(short.Parse(reg.Value.ToString()));

                if (_modbusServer.holdingRegisters[reg.ValueRegister] != BitConverter.ToInt16(buff4b, 0))
                {
                    _modbusServer.holdingRegisters[reg.ValueRegister] = BitConverter.ToInt16(buff4b, 0);
                    if (reg.NeedTwoRegisters)
                    {
                        _modbusServer.holdingRegisters[reg.SecondRegister] = BitConverter.ToInt16(buff4b, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write reg ex (group {groupName}, reg {reg.Path}): " + ex.Message);
                _eventLog.WriteEntry($"Write reg ex (group {groupName}, reg {reg.Path}): " + ex.Message);
            }
        }

        bool _isEnables = true;
        public void Start()
        {
            while (_isEnables)
            {
                Parallel.ForEach(config.Childs, GetValue);
                Thread.Sleep(config.Duration * 1000);
            }
        }
        public void Continue()
        {
            _isEnables = true;
        }
        public void Pause()
        {
            _isEnables = false;
        }
        public void Stop()
        {
            _isEnables = false;
            Dispose();
        }
        public void Dispose()
        {
            _modbusServer.StopListening();
        }

        public void ReConfigur() //In separate method
        {
            config.ConfingRead(_configFilePath);
        }
    }
}
