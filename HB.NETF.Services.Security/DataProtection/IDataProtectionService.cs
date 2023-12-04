using System.Security.Cryptography;

namespace HB.NETF.Services.Security.DataProtection {
    public interface IDataProtectionService {
        void SetEntropy(byte[] entropy);
        void SetScope(DataProtectionScope scope);
        byte[] Protect(byte[] data);
        byte[] Unprotect(byte[] data);
    }
}
