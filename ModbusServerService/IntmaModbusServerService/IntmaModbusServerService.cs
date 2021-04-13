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

namespace Intma.ModbusServerService
{
    public partial class IntmaModbusServerService : ServiceBase
    {
        public IntmaModbusServerService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            httpXmlReader.ReConfigur();
            Thread loggerThread = new Thread(new ThreadStart(httpXmlReader.Start));
            loggerThread.Start();
        }


        protected override void OnPause()
        {
            httpXmlReader.Pause();
            Thread.Sleep(1000);
        }
        protected override void OnContinue()
        {
            httpXmlReader.ReConfigur();
            httpXmlReader.Continue();
        }
        protected override void OnStop()
        {
            httpXmlReader.Stop();
            Thread.Sleep(1000);
        }
    }
}
