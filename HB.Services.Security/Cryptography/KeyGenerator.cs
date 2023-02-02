using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography {
    public static class KeyGenerator {
        public static AesKey GenerateAesKey(int keySize = 256) {
            Aes aes = Aes.Create();
            aes.KeySize = keySize;

            return new AesKey(aes.Key, aes.IV);
        }

        public static RsaKey[] GenerateRsaKeys(int keySizeInBits = 2048) {
            RSA rsa = RSA.Create(keySizeInBits);

            return new RsaKey[] { new RsaKey(rsa.ExportRSAPublicKey(), keySizeInBits, true), new RsaKey(rsa.ExportRSAPrivateKey(), keySizeInBits, false) };
        }

        public static RsaKey GeneratePublicKey(int keySizeInBits = 2048) {
            RSA rsa = RSA.Create(keySizeInBits);
            return new RsaKey(rsa.ExportRSAPublicKey(), keySizeInBits, true);
        }
        
        public static RsaKey GeneratePrivateKey(int keySizeInBits = 2048) {
            RSA rsa = RSA.Create(keySizeInBits);
            return new RsaKey(rsa.ExportRSAPrivateKey(), keySizeInBits, false);
        }
    }
}
