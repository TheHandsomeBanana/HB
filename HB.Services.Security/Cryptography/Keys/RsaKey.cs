using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Keys {
    public class RsaKey : IKey {
        public byte[] Key { get; set; }
        public bool IsPublic { get; set; }

        public RsaKey(byte[] key, bool isPublic) {
            Key = key;
            IsPublic = isPublic;
        }
    }
}
