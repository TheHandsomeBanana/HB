using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataHandling.Async;
using HB.NETF.Services.Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling.Options {
    internal class StreamOptionBuilder : IStreamOptionBuilder {
        StreamWrapper wrapper;
        public StreamOptionBuilder(StreamWrapper wrapper) {
            this.wrapper = wrapper;
        }


        public IStreamWrapper Set() {
            return wrapper;
        }

        public IAsyncStreamWrapper SetAsync() {
            if (wrapper.GetType().GetInterface(nameof(IAsyncStreamWrapper)) == null)
                throw new StreamWrapperException($"{nameof(StreamWrapper)} instance is not async.");

            return (IAsyncStreamWrapper)wrapper;
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
            wrapper.Options.EncryptionMode = encryptionMode;
            return this;
        }
    }
}
