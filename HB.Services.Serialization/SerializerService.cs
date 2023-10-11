using HB.Common;
using HB.Services.Serialization.Xml;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HB.Services.Serialization {
    public class SerializerService : ISerializerService {
        public string Serialize<T>(T data, SerializerMode mode) => Serialize(data, mode, null, null);
        public string Serialize<T>(T data, XmlSerializerSettings settings) => Serialize(data, SerializerMode.Xml, null, settings);
        public string Serialize<T>(T data, JsonSerializerSettings settings) => Serialize(data, SerializerMode.Json, settings, null);

        public T? Deserialize<T>(string data, SerializerMode mode) => Deserialize<T>(data, mode, null, null);
        public T? Deserialize<T>(string data, XmlSerializerSettings settings) => Deserialize<T>(data, SerializerMode.Xml, null, settings);
        public T? Deserialize<T>(string data, JsonSerializerSettings settings) => Deserialize<T>(data, SerializerMode.Json, settings, null);

        public byte[] GetResultBytes(string content) => GlobalEnvironment.Encoding.GetBytes(content);
        public string GetResultString(byte[] content) => GlobalEnvironment.Encoding.GetString(content);
        public string ToBase64(byte[] buffer) => Convert.ToBase64String(buffer);
        public byte[] FromBase64(string base64) => Convert.FromBase64String(base64);

        private static string Serialize<T>(T data, SerializerMode mode, JsonSerializerSettings? jsonSettings, XmlSerializerSettings? xmlSettings) {
            switch(mode) {
                case SerializerMode.Json:
                    return JsonConvert.SerializeObject(data, Formatting.Indented, jsonSettings);
                case SerializerMode.Xml:
                    return XmlConvert.SerializeObject(data, xmlSettings);
            }

            throw new NotSupportedException($"{mode} not supported.");
        }

        private static T? Deserialize<T>(string data, SerializerMode mode, JsonSerializerSettings? jsonSettings, XmlSerializerSettings? xmlSettings) {
            switch (mode) {
                case SerializerMode.Json:
                    return JsonConvert.DeserializeObject<T>(data, jsonSettings);
                case SerializerMode.Xml:
                    return XmlConvert.DeserializeObject<T>(data, xmlSettings);
            }

            throw new NotSupportedException($"{mode} not supported.");
        }
    }
}