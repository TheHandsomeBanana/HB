using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Interfaces {
    public interface IGenCryptoService<TValue> {
        public byte[] Encrypt(TValue data, IKey key);
        public TValue Decrypt(byte[] cipher, IKey key);

        public IKey[] GenerateKeys(int keySize);
    }

    public interface IGenCryptoService<TValue, TKey> : IGenCryptoService<TValue> where TKey : IKey {
        public byte[] Encrypt(TValue data, TKey key);
        public TValue Decrypt(byte[] cipher, TKey key);
        public new TKey[] GenerateKeys(int keySize);
    }
}
