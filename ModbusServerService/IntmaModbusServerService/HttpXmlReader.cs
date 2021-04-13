using EasyModbus;
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

        public string WebAdress { get; private set; }
        public string ModbusServerAdress { get; private set; }
        public int Port { get; private set; }
        public int Duration { get; set; }

        List<Intma.ModbusServerService.Configurator.Register> _registers;
        ModbusServer _modbusServer;
        System.Diagnostics.EventLog _eventLog;

        public HttpXmlReader()
        {
            _modbusServer = new ModbusServer();
            if (!System.Diagnostics.EventLog.Exists("IntmaModbusServerService EventLog"))
                System.Diagnostics.EventLog.CreateEventSource("IntmaModbusServerService", "IntmaModbusServerService EventLog");

            _eventLog = new System.Diagnostics.EventLog("IntmaModbusServerService EventLog");
            _eventLog.Source = "IntmaModbusServerService";

            ReConfigur();

            Console.WriteLine(Port);
            Console.WriteLine(WebAdress);
            Console.WriteLine(ModbusServerAdress);
            StartServer();
        }

        public List<Intma.ModbusServerService.Configurator.Register> XmlParse(StreamReader streamReader)
        {
            try
            {
                XDocument doc;
                if (streamReader == null)
                    doc = XDocument.Load("1.xml");
                else
                    doc = XDocument.Load(streamReader, LoadOptions.None);


                for(int i = 0; i< _registers.Count; i++)
                {
                    var arr = _registers[i].Path.Split(Intma.ModbusServerService.Configurator.Register.PathDel);
                    var el = doc.Element(arr[0]);
                    for(int j = 1; j < arr.Length; j++)
                    {
                        el = el.Element(arr[j]);
                    }
                 
                    _registers[i].Value = el.Value;
                }

                if(streamReader != null)
                    streamReader.Close();

                return _registers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xml Parse ex: " + ex.Message);
                _eventLog.WriteEntry("Xml Parse ex: " + ex.Message);
                return null;
            }
        }

        public void GetValue()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebAdress);
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
                            foreach (var el in XmlParse(readStream))
                                WriteValue(el);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Web Read ex: " + ex.Message);
                _eventLog.WriteEntry("Web Read ex: "  + ex.Message);
                foreach (var el in XmlParse(null))
                    WriteValue(el);
            }
        }

        public void StartServer()
        {
            try
            {
                _modbusServer.LocalIPAddress = IPAddress.Parse(ModbusServerAdress);
                _modbusServer.HoldingRegistersChanged += ModbusServer_HRChanged;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                _modbusServer.Listen();
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



                if (_modbusServer.holdingRegisters.localArray[writing.ValueRegister] != BitConverter.ToInt16(buff4b,0))
                {
                    _modbusServer.holdingRegisters.localArray[writing.ValueRegister] = BitConverter.ToInt16(buff4b, 0);
                    if(writing.NeedTwoRegisters)
                    {
                        _modbusServer.holdingRegisters.localArray[writing.SecondRegister] = BitConverter.ToInt16(buff4b, 2);
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
                Thread.Sleep(Duration * 1000);
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
            var doc = XDocument.Load(@"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config");
            var root = doc.Root;
            WebAdress = root.Element("WebAdress").Value;
            ModbusServerAdress = root.Element("ModbusServerAdress").Value;
            Port = Int32.Parse(root.Element("Port").Value);
            Duration = Int32.Parse(root.Element("Duration").Value);
            _registers = new List<Intma.ModbusServerService.Configurator.Register>();
            foreach (var el in root.Element("Registers").Elements())
            {
                _registers.Add(new Intma.ModbusServerService.Configurator.Register(el));
            }
        }
    }
}
