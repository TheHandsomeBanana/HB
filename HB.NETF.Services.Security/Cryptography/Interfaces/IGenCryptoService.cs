using HB.NETF.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Cryptography.Interfaces {
    public interface IGenCryptoService<TValue> {
        byte[] Encrypt(TValue data, IKey key);
        TValue Decrypt(byte[] cipher, IKey key);

        IKey[] GenerateKeys(int keySize);
    }

    public interface IGenCryptoService<TValue, TKey> : IGenCryptoService<TValue> where TKey : IKey {
        byte[] Encrypt(TValue data, TKey key);
        TValue Decrypt(byte[] cipher, TKey key);
        new TKey[] GenerateKeys(int keySize);
    }
}
