using HB.Common.Serialization;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.Cryptography.Keys;
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
            throw new NotImplementedException();
        }

        public TValue Decrypt(byte[] cipher, IKey key) {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(TValue data, RsaKey key) {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(TValue data, IKey key) {
            throw new NotImplementedException();
        }

        public RsaKey[] GenerateKeys(int keySize) {
            throw new NotImplementedException();
        }

        IKey[] IGenCryptoService<TValue>.GenerateKeys(int keySize) {
            throw new NotImplementedException();
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
            KeyGenerator keygen = new KeyGenerator();
            return keygen.GenerateRsaKeys(keySize);
        }

        IKey[] ICryptoService.GenerateKeys(int keySize) {
            return GenerateKeys(keySize);
        }

       
    }
}
