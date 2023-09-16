using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Identifier {
    public class Identifier {
        public Guid Id { get; }
        public object Reference { get; }
        internal Identifier(object reference) {
            Id = Guid.NewGuid();
            Reference = reference;
        }

        internal bool Identify(Guid guid) => guid == Id;
    }

    public class Identifier<TReference> {
        public Guid Id { get; }
        public TReference Reference { get; }
        internal Identifier(TReference reference) {
            Id = Guid.NewGuid();
            Reference = reference;
        }

        internal bool Identify(Guid guid) => guid == Id;
    }

    public class Identifier<TIdentifier, TReference> {
        public TIdentifier Id { get; }
        public TReference Reference { get; }

        public Identifier(TIdentifier id, TReference reference) {
            this.Id = id;
            this.Reference = reference;
        }

        internal bool Identify(TIdentifier id, Func<TIdentifier, TIdentifier, bool> comparer) => comparer(id, Id);
    }
}