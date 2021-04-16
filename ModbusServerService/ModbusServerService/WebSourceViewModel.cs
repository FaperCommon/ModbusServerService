﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class WebSourceViewModel: Notify, ICloneable
    {
        string _webAdress;
        ObservableCollection<RegistersGroupViewModel> _registers;
        int _duration;
        string _name;
        public bool IsWebSource { get; set; } = true;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(); } }
        public string WebAddress { get => $@"http:\\{_webAdress}"; set { _webAdress = value.Replace(@"http:\\", ""); OnPropertyChanged(); } }
        public string Type { get; } = "WebSourceViewModel";

        public ObservableCollection<RegistersGroupViewModel> Childs { get => _registers; set { _registers = value; OnPropertyChanged(); } }

        public WebSourceViewModel()
        {
            Duration = 5;
            Childs = new ObservableCollection<RegistersGroupViewModel>();
        }
        public WebSourceViewModel(XElement source)
        {
            Childs = new ObservableCollection<RegistersGroupViewModel>();
            WebAddress = source.Element("WebAddress").Value;
            Name = source.Element("Name").Value;

            foreach (var weight in source.Element("Weights").Elements())
            {
                Childs.Add(new RegistersGroupViewModel(weight));
            }
        }

        public object Clone()
        {
            var clone = (WebSourceViewModel)this.MemberwiseClone();
            clone.Childs = new ObservableCollection<RegistersGroupViewModel>();
            foreach (var source in Childs)
            {
                clone.Childs.Add((RegistersGroupViewModel)source.Clone());
            }
            return clone;
        }

        public string ExportTopicForKepServer()
        {
            StringBuilder outStr = new StringBuilder();
            outStr.Append("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value\n");
            foreach(var group in Childs)
            {
                outStr.Append(group.ToStringForKepServer());
            }
            return outStr.ToString();
        }

        public string ExportTopicForWonderware()
        {
            return String.Join("",Childs);
        }
    }
}
