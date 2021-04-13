using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Intma.ModbusServerService
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                IntmaModbusServerService service1 = new IntmaModbusServerService();
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new IntmaModbusServerService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
