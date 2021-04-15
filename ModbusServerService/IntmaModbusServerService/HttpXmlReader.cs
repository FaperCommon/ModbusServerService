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
    class HttpXmlReader: IDisposable
    {
        ConfigViewModel config;
        
        ModbusServer _modbusServer;
        System.Diagnostics.EventLog _eventLog;

        public HttpXmlReader()
        {
            _modbusServer = new ModbusServer();
            config = new ConfigViewModel();
            if (!System.Diagnostics.EventLog.Exists("IntmaModbusServerService EventLog"))
                System.Diagnostics.EventLog.CreateEventSource("IntmaModbusServerService", "IntmaModbusServerService EventLog");

            _eventLog = new System.Diagnostics.EventLog("IntmaModbusServerService EventLog");
            _eventLog.Source = "IntmaModbusServerService";

            ReConfigur();

            Console.WriteLine(config.Port);
            Console.WriteLine(config.ModbusServerAddress);
            StartServer();
        }
        
        public void GetValue()
        {
            try
            {
                foreach(var source in config.Childs) { 
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(source.WebAddress);
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
                                foreach (var el in XmlParse(readStream, source.Childs))
                                    WriteValue(el);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Web Read ex: " + ex.Message);
                _eventLog.WriteEntry("Web Read ex: "  + ex.Message);
            }
        }

        public IList<Register> XmlParse(StreamReader streamReader, IList<Register> registers)
        {
            try
            {
                XDocument doc;
                doc = XDocument.Load(streamReader, LoadOptions.None);


                for (int i = 0; i < registers.Count; i++)
                {
                    var arr = registers[i].Path.Split(Register.PathDel);
                    var el = doc.Element(arr[0]);
                    for (int j = 1; j < arr.Length; j++)
                    {
                        el = el.Element(arr[j]);
                    }

                    registers[i].Value = el.Value;
                }

                if (streamReader != null)
                    streamReader.Close();

                return registers;
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
                _modbusServer.HoldingRegistersChanged += ModbusServer_HRChanged;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                _modbusServer.Listen();
                Console.WriteLine(_modbusServer.LocalIPAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start server ex: "+ ex.Message);
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

        void WriteValue(Intma.ModbusServerService.Configurator.Register writing)
        {
            try
            {
                Console.WriteLine(writing.Value.ToString());
                byte[] buff4b;
                if (writing.NeedTwoRegisters)
                {
                    if(writing.SelectedDataType == "Float")
                    {
                        buff4b = BitConverter.GetBytes(Single.Parse(writing.Value.ToString()));
                    }
                    else if (writing.SelectedDataType == "Int")
                    {
                        buff4b = BitConverter.GetBytes(Int32.Parse(writing.Value.ToString()));
                    }
                    else if (writing.SelectedDataType == "Long")
                    {
                        buff4b = BitConverter.GetBytes(long.Parse(writing.Value.ToString()));
                    }
                    else
                        buff4b = BitConverter.GetBytes(short.Parse(writing.Value.ToString()));
                }
                else
                    buff4b = BitConverter.GetBytes(short.Parse(writing.Value.ToString()));
                
                if (_modbusServer.holdingRegisters[writing.ValueRegister] != BitConverter.ToInt16(buff4b,0))
                {
                    _modbusServer.holdingRegisters[writing.ValueRegister] = BitConverter.ToInt16(buff4b, 0);
                    if(writing.NeedTwoRegisters)
                    {
                        _modbusServer.holdingRegisters[writing.SecondRegister] = BitConverter.ToInt16(buff4b, 2);
                    }
                    Console.WriteLine(writing.Value.ToString() + " " + writing.ValueRegister);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write reg ex: "+ ex.Message);
                _eventLog.WriteEntry("Write reg ex: " + ex.Message);
            }
        }

        bool _isEnables = true;
        public void Start()
        {
            while (_isEnables)
            {
                GetValue();
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
            config.ConfingRead(@"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config");
        }
    }
}
