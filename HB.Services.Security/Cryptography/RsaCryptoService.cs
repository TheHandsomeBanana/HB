using HB.Common;
using HB.Common.Serialization;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Exceptions;
using HB_Utilities.Common.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography {
    public class RsaCryptoService<TValue> : IGenCryptoService<TValue, RsaKey> {
        private SerializerMode serializerMode;
        public RsaCryptoService(SerializerMode serializerMode) {
            if (serializerMode == SerializerMode.Binary)
                throw new NotSupportedException("Binary serialization is not supported.");

            this.serializerMode = serializerMode;
        }

        public TValue Decrypt(byte[] cipher, RsaKey key) {
            RSA rsa = RSA.Create(key.KeySize);
            if (key.IsPublic)
                throw new ArgumentException("Cannot decrypt with a public key.");

            rsa.ImportRSAPrivateKey(key.Key, out int bytesRead);
            byte[] targetBuffer = rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);

            TValue? target;
            string targetString = GlobalEnvironment.Encoding.GetString(targetBuffer);
            switch (serializerMode) {
                case SerializerMode.Json:
                    target = JsonConvert.DeserializeObject<TValue>(targetString);
                    break;
                case SerializerMode.Xml:
                    target = XmlConvert.DeserializeObject<TValue>(targetString);
                    break;
                default:
                    throw new NotSupportedException($"{serializerMode} is not supported.");
            }

            if (target == null)
                throw new CryptoServiceException("Decryption failed, deserialized target is null.");

            return target;
        }

        public TValue Decrypt(byte[] cipher, IKey key) {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));
            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Decrypt(cipher, (RsaKey)key);
        }

        public byte[] Encrypt(TValue data, RsaKey key) {
            string? targetString;

            switch (serializerMode) {
                case SerializerMode.Json:
                    targetString = JsonConvert.SerializeObject(data);
                    break;
                case SerializerMode.Xml:
                    targetString = XmlConvert.SerializeObject(data);
                    break;
                default:
                    throw new NotSupportedException($"{serializerMode} is not supported.");
            }

            if (targetString == null)
                throw new CryptoServiceException("Encryption failed, serialized data is null");

            RSA rsa = RSA.Create(key.KeySize);
            if (!key.IsPublic)
                throw new ArgumentException("Cannot encrypt with a private key.");

            rsa.ImportRSAPublicKey(key.Key, out int bytesRead);

            return rsa.Encrypt(GlobalEnvironment.Encoding.GetBytes(targetString), RSAEncryptionPadding.OaepSHA512);
        }

        public byte[] Encrypt(TValue data, IKey key) {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Encrypt(data, (RsaKey)key);
        }

        public RsaKey[] GenerateKeys(int keySize) {
            return KeyGenerator.GenerateRsaKeys(keySize);
        }

        IKey[] IGenCryptoService<TValue>.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }
    }

    public class RsaCryptoService : ICryptoService<RsaKey> {
        public byte[] Decrypt(byte[] cipher, IKey key) {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));
            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Decrypt(cipher, (RsaKey)key);
        }

        public byte[] Encrypt(byte[] data, IKey key) {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Encrypt(data, (RsaKey)key);
        }

        public byte[] Decrypt(byte[] cipher, RsaKey key) {
            RSA rsa = RSA.Create(key.KeySize);
            if (key.IsPublic)
                throw new ArgumentException("Cannot decrypt with a public key.");

            rsa.ImportRSAPrivateKey(key.Key, out int bytesRead);

            return rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);
        }

        public byte[] Encrypt(byte[] data, RsaKey key) {
            RSA rsa = RSA.Create(key.KeySize);
            if (!key.IsPublic)
                throw new ArgumentException("Cannot encrypt with a private key.");

            rsa.ImportRSAPublicKey(key.Key, out int bytesRead);

            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA512);
        }

        public RsaKey[] GenerateKeys(int keySize = 2048) {
            return KeyGenerator.GenerateRsaKeys(keySize);
        }

        IKey[] ICryptoService.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }


    }
}
