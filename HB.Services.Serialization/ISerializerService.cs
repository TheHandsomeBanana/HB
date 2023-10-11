using HB.Services.Serialization.Xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Serialization {
    public interface ISerializerService {
        string Serialize<T>(T data, SerializerMode mode);
        string Serialize<T>(T data, XmlSerializerSettings settings);
        string Serialize<T>(T data, JsonSerializerSettings settings);
        T? Deserialize<T>(string data, SerializerMode mode);
        T? Deserialize<T>(string data, XmlSerializerSettings settings);
        T? Deserialize<T>(string data, JsonSerializerSettings settings);
        byte[] GetResultBytes(string content);
        string GetResultString(byte[] content);
        string ToBase64(byte[] buffer);
        byte[] FromBase64(string base64);
    }
}
