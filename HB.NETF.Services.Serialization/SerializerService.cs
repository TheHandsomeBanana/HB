using HB.NETF.Common;
using Newtonsoft.Json;
using System;

namespace HB.NETF.Services.Serialization {
    public class SerializerService : ISerializerService {
        public T Deserialize<T>(string data, SerializerMode mode) {
            switch (mode) {
                case SerializerMode.Json:
                    return JsonConvert.DeserializeObject<T>(data);
            }

            throw new NotSupportedException($"{mode} not supported.");
        }

        public string Serialize<T>(T data, SerializerMode mode) {
            switch (mode) {
                case SerializerMode.Json:
                    return JsonConvert.SerializeObject(data, Formatting.Indented);
            }

            throw new NotSupportedException($"{mode} not supported.");
        }

        public byte[] SerializeToByte<T>(T data, SerializerMode mode) {
            return GetResultBytes(Serialize(data, mode));
        }

        public T DeserializeFromByte<T>(byte[] data, SerializerMode mode) {
            return Deserialize<T>(GetResultString(data), mode);
        }

        public byte[] GetResultBytes(string content) => GlobalEnvironment.Encoding.GetBytes(content);
        public string GetResultString(byte[] content) => GlobalEnvironment.Encoding.GetString(content);
        public string ToBase64(byte[] buffer) => Convert.ToBase64String(buffer);
        public byte[] FromBase64(string base64) => Convert.FromBase64String(base64);
    }
}