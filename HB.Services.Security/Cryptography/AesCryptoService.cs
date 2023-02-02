using HB.Common;
using HB.Common.Serialization;
using HB.Common.Serialization.Streams;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography
{

    public class AesCryptoService<TValue> : IGenCryptoService<TValue, AesKey> {
        private SerializerMode serializerMode;
        public AesCryptoService(SerializerMode serializerMode) {
            if (serializerMode == SerializerMode.Binary)
                throw new NotSupportedException("Binary serialization is not supported.");

            this.serializerMode = serializerMode;
        }

        public TValue Decrypt(byte[] cipher, AesKey key) {
            throw new NotImplementedException();
        }

        public TValue Decrypt(byte[] cipher, IKey key) {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(TValue data, AesKey key) {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(TValue data, IKey key) {
            throw new NotImplementedException();
        }

        public AesKey[] GenerateKeys(int keySize) {
            throw new NotImplementedException();
        }

        IKey[] IGenCryptoService<TValue>.GenerateKeys(int keySize) {
            throw new NotImplementedException();
        }
    }

    public class AesCryptoService : ICryptoService<AesKey> {

        public byte[] Decrypt(byte[] cipher, IKey key) {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));
            if (!(key is AesKey))
                throw new ArgumentException($"The provided key is not an {nameof(AesKey)}.");

            return Decrypt(cipher, (AesKey)key);
        }

        public byte[] Encrypt(byte[] data, IKey key) {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
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
