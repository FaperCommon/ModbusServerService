using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Intma.ModbusServerService
{
    public partial class IntmaModbusServerService : ServiceBase
    {
        public IntmaModbusServerService()
        {
            InitializeComponent();
            this.CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            Thread serviceThread = new Thread(new ThreadStart(InitTimer));
            serviceThread.Start();
        }

        System.Timers.Timer timer;
        protected void InitTimer() {

            httpXmlReader = new HttpXmlReader();
            timer = new System.Timers.Timer();
            timer.Interval = httpXmlReader.Config.Duration * 1000; 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            httpXmlReader.UpdateValue();
        }

        protected override void OnStop()
        {
            timer.Stop();
            httpXmlReader.Dispose();
        }
    }
}
