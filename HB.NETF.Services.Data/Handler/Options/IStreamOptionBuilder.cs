using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Data.Handler.Options {
    public interface IStreamOptionBuilder {

        IStreamHandler Set();
        IAsyncStreamHandler SetAsync();
        IStreamOptionBuilder UseCryptography(EncryptionMode encryptionMode);
        IStreamOptionBuilder ProvideKey(IKey key);
        IStreamOptionBuilder UseBase64();
    }
}
