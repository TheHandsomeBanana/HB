using System;
using System.Xml.Serialization;

namespace HB.NETF.Services.Serialization.Xml {
    public class XmlSerializerSettings {
        public XmlSerializerNamespaces XmlSerializerNamespaces { get; set; } = new XmlSerializerNamespaces();
        public XmlAttributeOverrides XmlAttributeOverrides { get; set; }
        public Type[] ExtraTypes { get; set; }
        public XmlRootAttribute Root { get; set; }
        public string DefaultNamespace { get; set; }
        public string Location { get; set; }
        public string EncodingStyle { get; set; }
        public XmlDeserializationEvents XmlDeserializationEvents { get; set; } = new XmlDeserializationEvents();
    }
}
