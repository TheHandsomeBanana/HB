using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Handler.Options {
    internal class StreamOptions {
        public EncryptionMode? EncryptionMode { get; set; }
        public IKey Key { get; set; }
        public bool UseBase64 { get; set; }
        public bool UseEncryption{ get; set; }

    }
}
