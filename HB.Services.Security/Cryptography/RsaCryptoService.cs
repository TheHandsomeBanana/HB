using HB.Common;
using HB.Services.Serialization;
using HB.Services.Serialization.Xml;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography {
    public class RsaCryptoService : IRsaCryptoService {
        public byte[] Decrypt(byte[] cipher, IKey key) {
            ArgumentNullException.ThrowIfNull(cipher, nameof(cipher));
            if (key is not RsaKey)
                throw new ArgumentException($"The provided key is not an {nameof(RsaKey)}.");

            return Decrypt(cipher, (RsaKey)key);
        }

        public byte[] Encrypt(byte[] data, IKey key) {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            if (key is not RsaKey)
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

        public RSA GetNewRSA(int keySizeInBits = 2048) {
            return RSA.Create(keySizeInBits);
        }

        public RsaKey GeneratePublicKey(RSA rsa) {
            return new RsaKey(rsa.ExportRSAPublicKey(), rsa.KeySize, true);
        }

        public RsaKey GeneratePrivateKey(RSA rsa) {
            return new RsaKey(rsa.ExportRSAPrivateKey(), rsa.KeySize, false);
        }
    }
}
