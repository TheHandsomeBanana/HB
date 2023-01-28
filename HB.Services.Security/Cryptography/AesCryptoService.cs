using HB.Common.Streams;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography {
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
                    return cs.Read(cipher.Length);
                }
            }
        }

        public byte[] Encrypt(byte[] data, AesKey key) {
            ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key.Key, key.IV);

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    cs.Write(data);
                }

                return ms.ToArray();
            }
        }
    }
}
