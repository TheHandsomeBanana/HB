using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.DataProtection {
    public interface IDataProtectionService {
        void SetEntropy(byte[] entropy);
        void SetScope(DataProtectionScope scope);
        byte[] Protect(byte[] data);
        byte[] Unprotect(byte[] data);
    }
}
