using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataHandling.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling.Options {
    public interface IStreamOptionBuilder {

        IStreamWrapper Set();
        IAsyncStreamWrapper SetAsync();
        IStreamOptionBuilder UseCryptography(EncryptionMode encryptionMode);
        IStreamOptionBuilder ProvideKey(IKey key);
        IStreamOptionBuilder UseBase64();
    }
}
