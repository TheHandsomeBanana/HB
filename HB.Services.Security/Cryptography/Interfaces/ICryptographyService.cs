using HB.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Interfaces {
    public interface ICryptographyService {
        public Task<byte[]> EncryptAsync(byte[] data, Key key);
        public Task<byte[]> DecryptAsync(byte[] cipher, Key key);
    }
}
