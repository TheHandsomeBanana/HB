using HB.NETF.Services.Security.Identifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.DataHandling {
    public class IdentifierStreamHandler<TIdentifier> : SecurityStreamHandler, ISecurityStreamHandler<IIdentifier<TIdentifier>> {
        public void Dispose() {
            this.Stream?.Dispose();
        }

        public IIdentifier<TIdentifier> Read() {
            throw new NotImplementedException();
        }

        public Task<IIdentifier<TIdentifier>> ReadAsync() {
            throw new NotImplementedException();
        }

        public void Write(IIdentifier<TIdentifier> item) {
            throw new NotImplementedException();
        }

        public Task WriteAsync(IIdentifier<TIdentifier> item) {
            throw new NotImplementedException();
        }
    }
}
