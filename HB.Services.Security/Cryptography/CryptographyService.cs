using HB.Common.Streams;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HB.Services.Security.Cryptography {
    public class CryptographyService : ICryptographyService {
        public async Task<byte[]> EncryptAsync(byte[] data, Key key) {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key.Content, key.IV);

            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    await cs.WriteAsync(data);
                }

                return ms.ToArray();
            }
        }

        public async Task<byte[]> DecryptAsync(byte[] cipher, Key key) {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));

            ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key.Content, key.IV);

            using (MemoryStream ms = new MemoryStream(cipher)) {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    return await cs.ReadAsync(cipher.Length);
                }
            }
        }
    }
}
