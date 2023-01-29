using HB_Utilities.Common.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HB.Services.DiskStorage.Objects {
    public class MemoryObject {
        private string? serializedObj;
        private object? memoryObj;
        private MemorySerializerType serializerType;

        public MemoryObject(object memoryObj, MemorySerializerType serializerType) {
            this.memoryObj = memoryObj;
            this.serializerType = serializerType;
        }

        public MemoryObject(string serializedObj, MemorySerializerType serializerType) {
            this.serializedObj = serializedObj;
            this.serializerType = serializerType;
        }

        public object? Get() => memoryObj;

        public string? Serialize() {
            switch (serializerType) {
                case MemorySerializerType.Json:
                    serializedObj = JsonConvert.SerializeObject(memoryObj);
                    break;
                case MemorySerializerType.Xml:
                    serializedObj = XmlConvert.SerializeObject(memoryObj);
                    break;
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }

            return serializedObj;
        }

        public object? Deserialize(Type objectType) {
            if (serializedObj == null)
                return null;

            switch (serializerType) {
                case MemorySerializerType.Json:
                    memoryObj = JsonConvert.DeserializeObject(serializedObj, objectType);
                    break;
                case MemorySerializerType.Xml:
                    memoryObj = XmlConvert.DeserializeObject(serializedObj, objectType);
                    break;
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }

            return memoryObj;
        }
    }

    public class MemoryObject<TObjectType> {
        private string? serializedObj;
        private TObjectType? memoryObj;
        private MemorySerializerType serializerType;

        public MemoryObject(TObjectType memoryObj, MemorySerializerType serializerType) {
            this.memoryObj = memoryObj;
            this.serializerType = serializerType;
        }

        public MemoryObject(string serializedObj, MemorySerializerType serializerType) {
            this.serializedObj = serializedObj;
            this.serializerType = serializerType;
        }

        public TObjectType? Get() => memoryObj;

        public string? Serialize() {
            switch (serializerType) {
                case MemorySerializerType.Json:
                    serializedObj = JsonConvert.SerializeObject(memoryObj);
                    break;
                case MemorySerializerType.Xml:
                    serializedObj = XmlConvert.SerializeObject(memoryObj);
                    break;
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }

            return serializedObj;
        }

        public TObjectType? Deserialize() {
            if (serializedObj == null)
                return default;

            switch (serializerType) {
                case MemorySerializerType.Json:
                    memoryObj = JsonConvert.DeserializeObject<TObjectType>(serializedObj);
                    break;
                case MemorySerializerType.Xml:
                    memoryObj = XmlConvert.DeserializeObject<TObjectType>(serializedObj);
                    break;
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }

            return memoryObj;
        }
    }
}
