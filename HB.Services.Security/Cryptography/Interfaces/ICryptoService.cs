using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Interfaces {
    public interface ICryptoService {
        public byte[] Encrypt(byte[] data, IKey key);
        public byte[] Decrypt(byte[] cipher, IKey key);

        public IKey[] GenerateKeys(int keySize);
    }

    public interface ICryptoService<TKey> : ICryptoService where TKey : IKey {
        public byte[] Encrypt(byte[] data, TKey key);
        public byte[] Decrypt(byte[] cipher, TKey key);

        public new TKey[] GenerateKeys(int keySize);
    }
}
