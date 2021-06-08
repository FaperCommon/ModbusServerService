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
        ModbusServer _modbusServer;
        System.Diagnostics.EventLog _eventLog;
       // string _configFilePath = @"C:\INTMABW500MBTCPService\INTMABW500MBTCPService.config";
       // string _logFilePath = @"C:\INTMABW500MBTCPService\IntmaModbusServerService.log";
        public Config Config { get; }

        public HttpXmlReader()
        {
            try
            {
                EventLogInit();
                _modbusServer = new ModbusServer();
                Config = new Config();
                ReConfigur();
                StartServer();
            }
            catch(Exception ex)
            {
                _eventLog.WriteEntry($"Constructor ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        private void EventLogInit()
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists("IntmaModbusServerServiceSource"))
                System.Diagnostics.EventLog.CreateEventSource("IntmaModbusServerServiceSource", "IntmaModbusServerService_EventLog");

                _eventLog = new System.Diagnostics.EventLog()
                {
                    Log = "IntmaModbusServerService_EventLog",
                    Source = "IntmaModbusServerServiceSource"
                };
            }
            catch
            {
            }
        }

        public void GetValue(WebSource webSource)
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
                            foreach (var el in XmlParse(readStream, webSource.RegistersGroups))
                                foreach (var reg in el.Registers)
                                    WriteValue(reg, el.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry($"Web Read ex {webSource.WebAddress}: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        public IList<RegistersGroup> XmlParse(StreamReader streamReader, IList<RegistersGroup> registersGroups)
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
                _eventLog.WriteEntry("Xml Parse ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                return new List<RegistersGroup>();
            }
        }

        public void StartServer()
        {
            try
            {
               // _modbusServer.LogFileFilename = _logFilePath;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                _modbusServer.Listen();
                _eventLog.WriteEntry(_modbusServer.LocalIPAddress + ", Server started!");
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry("Start server ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        void WriteValue(Intma.ModbusServerService.Configurator.Register reg, string groupName)
        {
            if (reg == null || String.IsNullOrEmpty(reg.Value.ToString()))
                return;
            try
            {
                byte[] buff4b;
                if (reg.NeedTwoRegisters)
                {
                    if (reg.DataType == "Float")
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
                _eventLog.WriteEntry($"Write reg ex (group {groupName}, reg {reg.Path}): " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        public void UpdateValue()
        {
            try { 
                Parallel.ForEach(Config.WebSources, GetValue);
            }
            catch(Exception ex)
            {
                _eventLog.WriteEntry($"UpdateValue ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        public void Dispose()
        {
            _modbusServer.StopListening();
        }

        public void ReConfigur() 
        {
            Config.ConfingRead();
        }
    }
}
