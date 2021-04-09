
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Intma.ModbusServerService
{
    partial class Service1
    {


        //Logger logger;
        private System.ComponentModel.IContainer components = null;
        HttpXmlReader httpXmlReader;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code


        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "Service1";
            httpXmlReader = new HttpXmlReader();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
        #endregion  
    }

    //class Logger
    //{
    //    FileSystemWatcher watcher;
    //    object obj = new object();
    //    bool enabled = true;
    //    public Logger()
    //    {
    //        watcher = new FileSystemWatcher("D:\\Temp");
    //        watcher.Deleted += Watcher_Deleted;
    //        watcher.Created += Watcher_Created;
    //        watcher.Changed += Watcher_Changed;
    //        watcher.Renamed += Watcher_Renamed;
    //    }

    //    public void Start()
    //    {
    //        watcher.EnableRaisingEvents = true;
    //        while (enabled)
    //        {
    //            Thread.Sleep(1000);
    //        }
    //    }
    //    public void Stop()
    //    {
    //        watcher.EnableRaisingEvents = false;
    //        enabled = false;
    //    }
    //    // переименование файлов
    //    private void Watcher_Renamed(object sender, RenamedEventArgs e)
    //    {
    //        string fileEvent = "переименован в " + e.FullPath;
    //        string filePath = e.OldFullPath;
    //        RecordEntry(fileEvent, filePath);
    //    }
    //    // изменение файлов
    //    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    //    {
    //        string fileEvent = "изменен";
    //        string filePath = e.FullPath;
    //        RecordEntry(fileEvent, filePath);
    //    }
    //    // создание файлов
    //    private void Watcher_Created(object sender, FileSystemEventArgs e)
    //    {
    //        string fileEvent = "создан";
    //        string filePath = e.FullPath;
    //        RecordEntry(fileEvent, filePath);
    //    }
    //    // удаление файлов
    //    private void Watcher_Deleted(object sender, FileSystemEventArgs e)
    //    {
    //        string fileEvent = "удален";
    //        string filePath = e.FullPath;
    //        RecordEntry(fileEvent, filePath);
    //    }

    //    private void RecordEntry(string fileEvent, string filePath)
    //    {
    //        lock (obj)
    //        {
    //            using (StreamWriter writer = new StreamWriter("D:\\templog.txt", true))
    //            {
    //                writer.WriteLine(String.Format("{0} файл {1} был {2}",
    //                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
    //                writer.Flush();
    //            }
    //        }
    //    }
    //}
}
