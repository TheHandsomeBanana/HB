﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Serialization {
    public interface ISerializerService {
        string Serialize<T>(T data, SerializerMode mode);
        byte[] SerializeToByte<T>(T data, SerializerMode mode);   
        T? Deserialize<T>(string data, SerializerMode mode);
        T? DeserializeFromByte<T>(byte[] data, SerializerMode mode);
        byte[] GetResultBytes(string content);
        string GetResultString(byte[] content);
        string ToBase64(byte[] buffer);
        byte[] FromBase64(string base64);
    }
}
