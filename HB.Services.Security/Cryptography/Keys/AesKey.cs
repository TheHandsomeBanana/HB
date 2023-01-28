using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Keys {
    public class AesKey : IKey {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }

        public AesKey(byte[] key, byte[] iV) {
            Key = key;
            IV = iV;
        }
    }
}
