using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling.Options {
    internal class StreamOptions {
        public EncryptionMode? EncryptionMode { get; set; }
        public IKey Key { get; set; }
        public bool UseBase64 { get; set; }

    }
}
