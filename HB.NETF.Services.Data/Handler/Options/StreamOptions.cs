using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;

namespace HB.NETF.Services.Data.Handler.Options {
    internal class StreamOptions {
        public EncryptionMode? EncryptionMode { get; set; }
        public IKey Key { get; set; }
        public bool UseBase64 { get; set; }
        public bool UseEncryption { get; set; }

    }
}
