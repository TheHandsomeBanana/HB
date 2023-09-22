using HB.NETF.Common;
using HB.NETF.Common.Extensions;
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
    public class AesCryptoService : IAesCryptoService {

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

        public AesKey GenerateKey(int keySize = 256) => KeyGenerator.GenerateAesKey(keySize);
    }
}
