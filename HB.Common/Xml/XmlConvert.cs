using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HB_Utilities.Common.Serialization {
    public static class XmlConvert {
        public static string? SerializeObject(object? o, XmlSerializerSettings? settings = null) {
            if (settings == null)
                settings = new XmlSerializerSettings();

            if (o == null)
                return null;

            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType(), settings.XmlAttributeOverrides, settings.ExtraTypes, settings.Root, settings.DefaultNamespace, settings.Location);
            StringBuilder sb = new StringBuilder();
            using (XmlWriter sw = XmlWriter.Create(sb)) {
                xmlSerializer.Serialize(sw, o, settings.XmlSerializerNamespaces, settings.EncodingStyle);
                return sb.ToString();
            }
        }

        public static object? DeserializeObject(string xml, Type type, XmlSerializerSettings? settings = null) {
            if (settings == null)
                settings = new XmlSerializerSettings();

            XmlSerializer xmlSerializer = new XmlSerializer(type, settings.XmlAttributeOverrides, settings.ExtraTypes, settings.Root, settings.DefaultNamespace, settings.Location);
            using (StringReader sr = new StringReader(xml)) {
                using (XmlReader xr = XmlReader.Create(sr)) {
                    return xmlSerializer.Deserialize(xr, settings.EncodingStyle, settings.XmlDeserializationEvents);
                }
            }
        }

        public static TXml? DeserializeObject<TXml>(string xml, XmlSerializerSettings? settings = null) {
            return (TXml?)DeserializeObject(xml, typeof(TXml), settings);
        }
    }
}
