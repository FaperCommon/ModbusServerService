using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace Intma.ModbusServerService.Configurator
{
    public class WebSource: ICloneable
    {
        string _webAdress;
        public bool IsWebSource { get; set; } = true;
        public int Duration { get; set; }
        public string WebAddress { get => $@"http:\\{_webAdress}"; set { _webAdress = value.Replace(@"http:\\", "");} }
        public string Type { get => nameof(WebSource); }

        public ObservableCollection<RegistersGroup> RegistersGroups { get; set; }

        public WebSource()
        {
            Duration = 5;
            RegistersGroups = new ObservableCollection<RegistersGroup>();
        }

        public WebSource(XElement source)
        {
            RegistersGroups = new ObservableCollection<RegistersGroup>();
            WebAddress = source.Element("WebAddress").Value;
            Duration = Int32.Parse(source.Element("Duration").Value);

            foreach (var weight in source.Element("Weights").Elements())
            {
                RegistersGroups.Add(new RegistersGroup(weight));
            }
        }

        public object Clone()
        {
            var clone = (WebSource)this.MemberwiseClone();
            clone.RegistersGroups = new ObservableCollection<RegistersGroup>();
            foreach (var source in RegistersGroups)
            {
                clone.RegistersGroups.Add((RegistersGroup)source.Clone());
            }
            return clone;
        }
        
        public string ExportTopic(TopicType type)
        {
            if (type == TopicType.Suitelink)
                return String.Join("", RegistersGroups);
            else //if (type == TopicType.KeepServer)
            {
                StringBuilder outStr = new StringBuilder();
                outStr.Append("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value\n");
                foreach (var group in RegistersGroups)
                {
                    outStr.Append(group.ToStringForKepServer());
                }
                return outStr.ToString();
            }
        }
    }
}
