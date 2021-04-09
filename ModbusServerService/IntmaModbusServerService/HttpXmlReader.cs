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

        public HttpXmlReader()
        {
            modbusServer = new ModbusServer();
            modbusClient = new ModbusClient();

            //IRule rule = FirewallManager.Instance.CreateApplicationRule(FirewallManager.Instance.GetProfile().Type, "Service1", FirewallAction.Allow, Assembly.GetExecutingAssembly().Location);
            //rule.Profiles = FirewallProfiles.Public | FirewallProfiles.Private;
            //FirewallManager.Instance.Rules.Add(rule);

            ReConfigur();
            Console.WriteLine(Port);
            Console.WriteLine(WebAdress);
            Console.WriteLine(ModbusServerAdress);
            StartServer();
        }


        System.Diagnostics.EventLog eventLog1 = new System.Diagnostics.EventLog();

        public string WebAdress { get; private set; }
        public string ModbusServerAdress { get; private set; }
        public int Port { get; private set; }
        public int Duration { get; set; }

        public List<Intma.ModbusServerService.Configurator.Register> Registers;
        ModbusServer modbusServer;
        ModbusClient modbusClient;

        public List<Intma.ModbusServerService.Configurator.Register> XmlParse(StreamReader streamReader)
        {
            try
            {
                var doc = XDocument.Load("1.xml");
                for(int i = 0; i< Registers.Count; i++)
                {
                    var arr = Registers[i].Path.Split(Registers[i].PathDel);
                    var el = doc.Element(arr[0]);
                    for(int j = 1; j < arr.Length; j++)
                    {
                        el = el.Element(arr[j]);
                    }
                 
                    if (Registers[i].IsFloat)
                    {
                        Registers[i].Value = Int32.Parse(el.Value.Split('.')[0]);
                        Registers[i].Mantissa = Int32.Parse(el.Value.Split('.')[1]);
                    }
                    else
                        Registers[i].Value = Int32.Parse(el.Value.Split('.')[0]);
                }

                //streamReader.Close();
                return Registers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xml Parse ex: " + ex.Message);
                return null;
            }
        }

        public void GetValue()
        {
            try
            {
                /*
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

                            modbusClient.Connect();
                            foreach (var el in XmlParse(readStream))
                                WriteValue(el);
                            modbusClient.Disconnect();
                        }
                    }
                }*/
                modbusClient.Connect();
                foreach (var el in XmlParse(null))
                    WriteValue(el);
                modbusClient.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Web Read Ex" + ex.Message);
            }
        }
        bool isEnables = true;
        public void Start()
        {
            while (isEnables)
            {
                GetValue();
                Thread.Sleep(Duration * 1000);
            }
        }
        public void Stop()
        {
            isEnables = false;//Disp
        }
        public void StartServer()
        {
            try
            {
                modbusServer.LocalIPAddress = IPAddress.Parse(ModbusServerAdress);
                modbusClient = new ModbusClient();
                modbusClient.IPAddress = ModbusServerAdress;
                modbusServer.HoldingRegistersChanged += ModbusServer_CoilsChanged;
            
                modbusServer.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start server ex: "+ ex.Message);
            }
        }

        private void ModbusServer_CoilsChanged(int coil, int numberOfCoils)
        {
            try
            {
                //MessageBox.Show("ModbusServer_CoilsChanged");
                //modbusClient.Connect();                                                    //Connect to Server //Write Coils starting with Address 5
                Console.WriteLine("ModbusServer_CoilsChanged");
                //Value = modbusClient.ReadHoldingRegisters(Reg, 1)[0].ToString();    //Read 10 Holding Registers from Server, starting with Address 1
                //modbusClient.Disconnect();                      //Read 10 Coils from Server, starting with address 10
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void WriteValue(Intma.ModbusServerService.Configurator.Register writing)
        {
            try
            {
                Console.WriteLine(writing.Value.ToString() +" " +writing.ValueRegister);
                modbusClient.WriteMultipleRegisters(writing.ValueRegister, new int[] { writing.Value });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write reg ex: "+ ex.Message);
            }
        }



        public void Dispose()
        {
            modbusServer.StopListening();
        }


        public void ReConfigur() //Вынести в отдельынй метод XML класса
        {
            var doc = XDocument.Load(@"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config");
            var root = doc.Root;
            WebAdress = root.Element("WebAdress").Value;
            ModbusServerAdress = root.Element("ModbusServerAdress").Value;
            Port = Int32.Parse(root.Element("Port").Value);
            Duration = Int32.Parse(root.Element("Duration").Value);
            Registers = new List<Intma.ModbusServerService.Configurator.Register>();
            foreach (var el in root.Element("Registers").Elements())
            {
                Registers.Add(new Intma.ModbusServerService.Configurator.Register(el));
            }

        }
    }
}
