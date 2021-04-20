
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Intma.ModbusServerService
{
    partial class IntmaModbusServerService
    {
        private System.ComponentModel.IContainer _components = null;
        HttpXmlReader httpXmlReader;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            _components = new System.ComponentModel.Container();
            this.ServiceName = "IntmaModbusServerService";
        }
        #endregion  
    }
}
