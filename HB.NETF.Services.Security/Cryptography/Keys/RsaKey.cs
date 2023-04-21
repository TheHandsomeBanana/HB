using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Cryptography.Keys {
    public class RsaKey : IKey {
        public RSAParameters Key { get; set; }
        public int KeySize { get; set; }
        public bool IsPublic { get; set; }

        public string Name => nameof(RsaKey);

        public RsaKey(RSAParameters key, int keySize, bool isPublic) {
            Key = key;
            KeySize = keySize;
            IsPublic = isPublic;
        }
    }
}
