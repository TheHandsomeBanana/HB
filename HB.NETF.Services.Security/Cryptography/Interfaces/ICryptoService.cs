using HB.NETF.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Cryptography.Interfaces {
    public interface ICryptoService {

        byte[] Encrypt(byte[] data, IKey key);
        byte[] Decrypt(byte[] cipher, IKey key);

        IKey[] GenerateKeys(int keySize);
    }
}
