using HB.NETF.Common;
using HB.NETF.Common.Serialization;
using HB.NETF.Common.Serialization.Streams;
using HB.NETF.Common.Serialization.Xml;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Cryptography {
    public class AesCryptoService<TValue> : IGenCryptoService<TValue, AesKey> {
        private SerializerMode serializerMode;
        public AesCryptoService(SerializerMode serializerMode) {
            if (serializerMode == SerializerMode.Binary)
                throw new NotSupportedException("Binary serialization is not supported.");

            this.serializerMode = serializerMode;
        }

        public TValue Decrypt(byte[] cipher, AesKey key) {
            ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key.Key, key.IV);
            TValue target;

            using (MemoryStream ms = new MemoryStream(cipher)) {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    byte[] cipherBuffer = cs.Read(cipher.Length);

                    // Remove trailing null chars
                    // Does not cover every case (Decrypted array could contain a valid trailing null char)
                    int i = cipherBuffer.Length - 1;
                    while (i >= 0 && cipherBuffer[i] == '\0')
                        i--;

                    byte[] targetBuffer = new byte[i + 1];
                    Array.Copy(cipherBuffer, targetBuffer, i + 1);

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
                }
            }

            if (target == null)
                throw new CryptoServiceException("Decryption failed, deserialized target is null.");

            return target;
        }

        public TValue Decrypt(byte[] cipher, IKey key) {
            if (cipher == null || cipher.Length == 0)
                throw new ArgumentNullException(nameof(cipher));

            if (!(key is AesKey))
                throw new ArgumentException($"The provided key is not an {nameof(AesKey)}.");

            return Decrypt(cipher, (AesKey)key);
        }

        public byte[] Encrypt(TValue data, AesKey key) {
            string targetString;

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

            ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    cs.Write(GlobalEnvironment.Encoding.GetBytes(targetString));
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        public byte[] Encrypt(TValue data, IKey key) {
           if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!(key is AesKey))
                throw new ArgumentException($"The provided key is not an {nameof(AesKey)}.");

            return Encrypt(data, (AesKey)key);
        }

        public AesKey[] GenerateKeys(int keySize) {
            return new AesKey[] { KeyGenerator.GenerateAesKey(keySize) };
        }

        IKey[] IGenCryptoService<TValue>.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }
    }

    public class AesCryptoService : ICryptoService<AesKey> {

        public byte[] Decrypt(byte[] cipher, IKey key) {
            if (cipher == null || cipher.Length == 0)
                throw new ArgumentNullException(nameof(cipher));

            if (!(key is AesKey))
                throw new ArgumentException($"The provided key is not an {nameof(AesKey)}.");

            return Decrypt(cipher, (AesKey)key);
        }

        public byte[] Encrypt(byte[] data, IKey key) {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!(key is AesKey))
                throw new ArgumentException($"The provided key is not an {nameof(AesKey)}.");

            return Encrypt(data, (AesKey)key);
        }

        public byte[] Decrypt(byte[] cipher, AesKey key) {
            ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream(cipher)) {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    byte[] cipherBuffer = cs.Read(cipher.Length);

                    // Remove trailing null chars
                    int i = cipherBuffer.Length - 1;
                    while (i >= 0 && cipherBuffer[i] == '\0')
                        i--;

                    byte[] targetBuffer = new byte[i + 1];
                    Array.Copy(cipherBuffer, targetBuffer, i + 1);
                    return targetBuffer;
                }
            }
        }

        public byte[] Encrypt(byte[] data, AesKey key) {
            ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    cs.Write(data);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        public async Task<byte[]> DecryptAsync(byte[] cipher, AesKey key) {
            ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream(cipher)) {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    byte[] cipherBuffer = await cs.ReadAsync(cipher.Length);

                    int i = cipherBuffer.Length - 1;
                    while (i >= 0 && cipherBuffer[i] == '\0')
                        i--;

                    byte[] targetBuffer = new byte[i + 1];
                    Array.Copy(cipherBuffer, targetBuffer, i + 1);
                    return targetBuffer;
                }
            }
        }

        public async Task<byte[]> EncryptAsync(byte[] data, AesKey key) {
            ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    await cs.WriteAsync(data);
                }

                return ms.ToArray();
            }
        }

        public AesKey[] GenerateKeys(int keySize = 256) {
            return new AesKey[] { KeyGenerator.GenerateAesKey(keySize) };
        }

        IKey[] ICryptoService.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }

    }
}
