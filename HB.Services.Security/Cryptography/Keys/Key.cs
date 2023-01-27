using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Cryptography.Keys {
    public class Key {
        public byte[] Content { get; set; }
        public byte[] IV { get; set; }

        public Key(byte[] content, byte[] iV) {
            Content = content;
            IV = iV;
        }
    }
}
