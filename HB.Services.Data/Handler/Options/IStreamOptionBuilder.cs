using HB.Services.Data.Handler.Async;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Handler.Options {
    public interface IStreamOptionBuilder {

        IStreamHandler Set();
        IAsyncStreamHandler SetAsync();
        IStreamOptionBuilder UseCryptography(EncryptionMode encryptionMode);
        IStreamOptionBuilder ProvideKey(IKey key);
        IStreamOptionBuilder UseBase64();
    }
}
