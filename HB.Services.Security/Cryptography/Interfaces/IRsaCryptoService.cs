using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Interfaces {
    public interface IRsaCryptoService : ICryptoService {
        byte[] Encrypt(byte[] data, RsaKey key);
        byte[] Decrypt(byte[] cipher, RsaKey key);

        RSA GetNewRSA(int keySizeInBits = 2048);
        RsaKey GeneratePublicKey(RSA rsa);
        RsaKey GeneratePrivateKey(RSA rsa);
    }
}
