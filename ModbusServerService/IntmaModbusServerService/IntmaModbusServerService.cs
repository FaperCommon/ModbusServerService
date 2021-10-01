using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace Intma.ModbusServerService
{
    public partial class IntmaModbusServerService : ServiceBase
    {
        public IntmaModbusServerService()
        {
            try
            {
                this.AutoLog = false;
                InitializeComponent();
                this.CanStop = true;

                if (!EventLog.SourceExists("IntmaModbusServerServiceSource"))
                {
                    EventLog.CreateEventSource("IntmaModbusServerServiceSource", "IntmaModbusServerService_EventLog");
                    EventLog.WriteEntry("Log Created!");
                }

                EventLog.Source = "IntmaModbusServerServiceSource";
                EventLog.Log = "IntmaModbusServerService_EventLog";
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"Construct ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread serviceThread = new Thread(new ThreadStart(InitTimer));
                serviceThread.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"OnStart ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        System.Timers.Timer timer;
        protected void InitTimer()
        {
            try
            {
                httpXmlReader = new HttpXmlReader(EventLog);
                timer = new System.Timers.Timer();
                timer.Interval = httpXmlReader.Config.Duration * 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
                timer.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"InitTimer ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                httpXmlReader.UpdateValue();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"OnTimer ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                timer.Stop();
                httpXmlReader.Dispose();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry($"OnStop ex: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
