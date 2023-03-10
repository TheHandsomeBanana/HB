using HB.Common;
using HB.Common.Serialization;
using HB.Common.Serialization.Xml;
using HB.Services.Security.Cryptography;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HB.Services.Storage {
    public struct MemoryObject {
        private string? serializedObj;
        private object? memoryObj;
        private SerializerMode serializerType;


        internal object? MemoryObj => memoryObj;
        internal string? SerializedObj => serializedObj;
        public bool IsSerialized => serializedObj != null;
        public bool IsDeserialized => memoryObj != null;

        public MemoryObject(object memoryObj, SerializerMode serializerType) {
            this.memoryObj = memoryObj;
            this.serializerType = serializerType;
        }

        internal MemoryObject(string serializedObj, SerializerMode serializerType) {
            this.serializedObj = serializedObj;
            this.serializerType = serializerType;
        }

        public string? Serialize() {
            serializedObj = SerializeInternal();

            if (serializedObj != null)
                memoryObj = null;

            return serializedObj;
        }

        public string? Serialize(IKey key) {
            string? obj = SerializeInternal();
            if (obj == null)
                return null;

            memoryObj = null;

            ICryptoService cryptoService = InitCryptoService(key.Name);
            serializedObj = Convert.ToBase64String(cryptoService.Encrypt(GlobalEnvironment.Encoding.GetBytes(obj), key));
            return serializedObj;
        }

        private string? SerializeInternal() {
            if (memoryObj == null)
                return null;

            switch (serializerType) {
                case SerializerMode.Json:
                    return JsonConvert.SerializeObject(memoryObj);
                case SerializerMode.Xml:
                    return XmlConvert.SerializeObject(memoryObj);
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }
        }

        public object? Deserialize(Type objectType) {
            if (serializedObj == null)
                return null;

            return DeserializeInternal(objectType, serializedObj);
        }

        public object? Deserialize(Type objectType, IKey key) {
            if (serializedObj == null)
                return null;

            ICryptoService cryptoService = InitCryptoService(key.Name);
            string obj = GlobalEnvironment.Encoding.GetString(cryptoService.Decrypt(Convert.FromBase64String(serializedObj), key));

            return DeserializeInternal(objectType, obj);
        }

        private object? DeserializeInternal(Type objectType, string obj) {
            switch (serializerType) {
                case SerializerMode.Json:
                    memoryObj = JsonConvert.DeserializeObject(obj, objectType);
                    break;
                case SerializerMode.Xml:
                    memoryObj = XmlConvert.DeserializeObject(obj, objectType);
                    break;
                default:
                    throw new NotSupportedException($"{serializerType} is not supported.");
            }

            serializedObj = null;

            return memoryObj;
        }

        private ICryptoService InitCryptoService(string keyType) {
            switch (keyType) {
                case nameof(AesKey):
                    return new AesCryptoService();
                case nameof(RsaKey):
                    return new RsaCryptoService();
                default:
                    throw new NotSupportedException($"Key type {keyType} is not supported");
            }
        }
    }

    public struct MemoryObject<TObjectType> {
        private string? serializedObj;
        private TObjectType? memoryObj;
        private SerializerMode serializerMode;

        internal TObjectType? MemoryObj => memoryObj;
        internal string? SerializedObj => serializedObj;
        public bool IsSerialized => serializedObj != null;
        public bool IsDeserialized => memoryObj != null;

        public MemoryObject(TObjectType memoryObj, SerializerMode serializerType) {
            this.memoryObj = memoryObj;
            serializerMode = serializerType;
        }

        internal MemoryObject(string serializedObj, SerializerMode serializerType) {
            this.serializedObj = serializedObj;
            serializerMode = serializerType;
        }

        public string? Serialize() {
            if (memoryObj == null)
                return null;

            switch (serializerMode) {
                case SerializerMode.Json:
                    serializedObj = JsonConvert.SerializeObject(memoryObj);
                    break;
                case SerializerMode.Xml:
                    serializedObj = XmlConvert.SerializeObject(memoryObj);
                    break;
                default:
                    throw new NotSupportedException($"{serializerMode} is not supported.");
            }

            return serializedObj;
        }

        public string? Serialize(IKey key) {
            if (memoryObj == null)
                return null;

            IGenCryptoService<TObjectType> cryptoService = InitCryptoService(key.Name);

            serializedObj = Convert.ToBase64String(cryptoService.Encrypt(memoryObj, key));
            return serializedObj;
        }

        public TObjectType? Deserialize() {
            if (serializedObj == null)
                return default;

            switch (serializerMode) {
                case SerializerMode.Json:
                    memoryObj = JsonConvert.DeserializeObject<TObjectType>(serializedObj);
                    break;
                case SerializerMode.Xml:
                    memoryObj = XmlConvert.DeserializeObject<TObjectType>(serializedObj);
                    break;
                default:
                    throw new NotSupportedException($"{serializerMode} is not supported.");
            }

            return memoryObj;
        }

        public TObjectType? Deserialize(IKey key) {
            if (serializedObj == null)
                return default;

            IGenCryptoService<TObjectType> cryptoService = InitCryptoService(key.Name);
            memoryObj = cryptoService.Decrypt(Convert.FromBase64String(serializedObj), key);
            return memoryObj;
        }

        private IGenCryptoService<TObjectType> InitCryptoService(string keyType) {
            switch (keyType) {
                case nameof(AesKey):
                    return new AesCryptoService<TObjectType>(serializerMode);
                case nameof(RsaKey):
                    return new RsaCryptoService<TObjectType>(serializerMode);
                default:
                    throw new NotSupportedException($"Key type {keyType} not supported");
            }
        }
    }
}
