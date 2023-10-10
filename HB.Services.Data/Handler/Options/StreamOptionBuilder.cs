using HB.Services.Data.Exceptions;
using HB.Services.Data.Handler.Async;
using HB.Services.Security.Cryptography.Keys;
using HB.Services.Security.Cryptography.Settings;
using HB.Services.Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Handler.Options {
    internal class StreamOptionBuilder : IStreamOptionBuilder {
        private readonly StreamHandler wrapper;
        public StreamOptionBuilder(StreamHandler wrapper) {
            this.wrapper = wrapper;
        }


        public IStreamHandler Set() {
            return wrapper;
        }

        public IAsyncStreamHandler SetAsync() {
            if (wrapper.GetType().GetInterface(nameof(IAsyncStreamHandler)) == null)
                throw new StreamHandlerException($"{nameof(StreamHandler)} instance is not async.");

            return (IAsyncStreamHandler)wrapper;
        }

        public IStreamOptionBuilder ProvideKey(IKey key) {
            wrapper.Options.Key = key;
            return this;
        }

        public IStreamOptionBuilder UseBase64() {
            wrapper.Options.UseBase64 = true;
            return this;
        }

        public IStreamOptionBuilder UseCryptography(EncryptionMode encryptionMode) {
            wrapper.Options.UseEncryption = true;
            wrapper.Options.EncryptionMode = encryptionMode;
            return this;
        }
    }
}
