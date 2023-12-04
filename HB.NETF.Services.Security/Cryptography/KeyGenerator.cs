using HB.NETF.Services.Security.Cryptography.Keys;
using System.Security.Cryptography;

namespace HB.NETF.Services.Security.Cryptography {
    public static class KeyGenerator {
        public static AesKey GenerateAesKey(int keySize = 256) {
            Aes aes = Aes.Create();
            aes.KeySize = keySize;

            return new AesKey(aes.Key, aes.IV);
        }

        public static RsaKey[] GenerateRsaKeys(int keySizeInBits = 2048) {
            RSA rsa = RSA.Create(keySizeInBits);


            return new RsaKey[] { new RsaKey(rsa.ExportParameters(false), keySizeInBits, true), new RsaKey(rsa.ExportParameters(true), keySizeInBits, false) };
        }

        public static RsaKey GeneratePublicKey(RSA rsa) {
            return new RsaKey(rsa.ExportParameters(false), rsa.KeySize, true);
        }

        public static RsaKey GeneratePrivateKey(RSA rsa) {
            return new RsaKey(rsa.ExportParameters(true), rsa.KeySize, false);
        }
    }
}
