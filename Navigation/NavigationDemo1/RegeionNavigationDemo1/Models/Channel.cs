using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RegeionNavigationDemo1.Models {

    public class Channel {

        [XmlAttribute,JsonProperty("name")] public string Name { get; set; }

        [XmlAttribute, JsonProperty("id")] public int Id { get; set; }

        [XmlAttribute, JsonProperty("type")] public string Type { get; set; }
    }
}
