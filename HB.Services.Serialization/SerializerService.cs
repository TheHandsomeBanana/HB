using HB.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HB.Services.Serialization {
    public class SerializerService : ISerializerService {
        public T? Deserialize<T>(string data, SerializerMode mode) {
            switch (mode) {
                case SerializerMode.Json:
                    return JsonConvert.DeserializeObject<T>(data);
                case SerializerMode.None:
                    break;
                case SerializerMode.Xml:
                    break;
            }

            throw new NotSupportedException($"{mode} not supported.");
        }

        public string Serialize<T>(T data, SerializerMode mode) {
            switch (mode) {
                case SerializerMode.Json:
                    return JsonConvert.SerializeObject(data, Formatting.Indented);
                case SerializerMode.None:
                    break;
                case SerializerMode.Xml:
                    break;
            }

            throw new NotSupportedException($"{mode} not supported.");
        }

        public byte[] SerializeToByte<T>(T data, SerializerMode mode) {
            return GetResultBytes(Serialize(data, mode));
        }

        public T? DeserializeFromByte<T>(byte[] data, SerializerMode mode) {
            return Deserialize<T>(GetResultString(data), mode);
        }

        public byte[] GetResultBytes(string content) => GlobalEnvironment.Encoding.GetBytes(content);
        public string GetResultString(byte[] content) => GlobalEnvironment.Encoding.GetString(content);
        public string ToBase64(byte[] buffer) => Convert.ToBase64String(buffer);
        public byte[] FromBase64(string base64) => Convert.FromBase64String(base64);
    }
}