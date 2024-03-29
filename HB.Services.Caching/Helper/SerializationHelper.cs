﻿using HB.Services.Serialization;
using HB.Services.Serialization.Json;
using HB.Services.Caching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HB.Services.Caching.Helper
{
    internal static class SerializationHelper {

        public static void Serialize(this Cache cache, FileStream fs) {
            Serialize(fs, cache.Value, cache.CacheType);
        }

        public static void Serialize(this CacheMetaInfo cacheMetaInfo, FileStream fs) {
            Serialize(fs, cacheMetaInfo, SerializerMode.Json);
        }

        public static object Deserialize(FileStream fs, Type objectType, SerializerMode ct) {
            switch (ct) {
                case SerializerMode.Binary:
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    return new BinaryFormatter().Deserialize(fs);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                case SerializerMode.Xml:
                    return new XmlSerializer(objectType).Deserialize(fs) ?? throw new NullReferenceException("Xml deserialization returned null.");
                case SerializerMode.Json:
                    return new JsonSerializer().Deserialize(fs, objectType) ?? throw new NullReferenceException("Json deserialization returned null."); ;
                default:
                    throw new NotSupportedException("Cache type is not supported.");
            }
        }

        private static void Serialize(FileStream fs, object value, SerializerMode ct) {
            switch (ct) {
                case SerializerMode.Binary:
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    new BinaryFormatter().Serialize(fs, value);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    break;
                case SerializerMode.Xml:
                    new XmlSerializer(value.GetType()).Serialize(fs, value);
                    break;
                case SerializerMode.Json:
                    new JsonSerializer().Serialize(fs, value);
                    break;
                default:
                    throw new NotSupportedException("Cache type is not supported.");
            }
        }
    }
}
