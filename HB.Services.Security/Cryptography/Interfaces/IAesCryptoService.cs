using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Interfaces {
    public interface IAesCryptoService : ICryptoService {
        byte[] Encrypt(byte[] data, AesKey key);
        byte[] Decrypt(byte[] cipher, AesKey key);
        AesKey GenerateKey(int keySize = 256);
    }
}
