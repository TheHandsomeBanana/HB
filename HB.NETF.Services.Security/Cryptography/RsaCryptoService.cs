using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using System;
using System.Security.Cryptography;

namespace HB.NETF.Services.Security.Cryptography {
    public class RsaCryptoService : IRsaCryptoService {
        public byte[] Decrypt(byte[] cipher, IKey key) {
            if (cipher == null || cipher.Length == 0)
                throw new ArgumentNullException(nameof(cipher));

            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Decrypt(cipher, (RsaKey)key);
        }

        public byte[] Encrypt(byte[] data, IKey key) {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!(key is RsaKey))
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Encrypt(data, (RsaKey)key);
        }

        public byte[] Decrypt(byte[] cipher, RsaKey key) {
            RSA rsa = RSA.Create(key.KeySize);
            if (key.IsPublic)
                throw new ArgumentException("Cannot decrypt with a public key.");


            rsa.ImportParameters(key.Key);

            return rsa.Decrypt(cipher, RSAEncryptionPadding.OaepSHA512);
        }

        public byte[] Encrypt(byte[] data, RsaKey key) {
            RSA rsa = RSA.Create(key.KeySize);
            if (!key.IsPublic)
                throw new ArgumentException("Cannot encrypt with a private key.");

            rsa.ImportParameters(key.Key);

            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA512);
        }

        public RsaKey[] GenerateKeys(int keySize = 2048) {
            return KeyGenerator.GenerateRsaKeys(keySize);
        }

        IKey[] ICryptoService.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }

        public RsaKey GeneratePublicKey(RSA rsa) {
            return new RsaKey(rsa.ExportParameters(false), rsa.KeySize, true);
        }

        public RsaKey GeneratePrivateKey(RSA rsa) {
            return new RsaKey(rsa.ExportParameters(true), rsa.KeySize, true);
        }

        public RSA GetNewRSA(int keySizeInBits = 2048) {
            return RSA.Create(keySizeInBits);
        }
    }
}
