using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.DataProtection {
    [SupportedOSPlatform("windows")]
    public class DataProtectionService : IDataProtectionService {
        private byte[] entropy = { 9, 8, 7, 6, 5 };
        private DataProtectionScope scope = DataProtectionScope.CurrentUser;
        public void SetEntropy(byte[] entropy) {
            this.entropy = entropy;
        }

        public void SetScope(DataProtectionScope scope) {
            this.scope = scope;
        }

        public byte[] Protect(byte[] data) {
            return ProtectedData.Protect(data, entropy, scope);
        }

        public byte[] Unprotect(byte[] data) {
            return ProtectedData.Unprotect(data, entropy, scope);            
        }
    }
}
