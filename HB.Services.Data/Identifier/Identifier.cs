using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Identifier {
    public class Identifier {
        public Guid Id { get; }
        public object Reference { get; }
        [JsonConstructor]
        private Identifier(Guid id, object reference) {
            this.Id = id;
            this.Reference = reference;
        }
        internal Identifier(object reference) {
            Id = Guid.NewGuid();
            Reference = reference;
        }

        public bool Identify(Guid guid) => guid == Id;
    }

    public class Identifier<TReference> {
        public Guid Id { get; }
        public TReference Reference { get; }
        [JsonConstructor]
        private Identifier(Guid id, TReference reference) {
            this.Id = id;
            this.Reference = reference;
        }

        internal Identifier(TReference reference) {
            Id = Guid.NewGuid();
            Reference = reference;
        }

        public bool Identify(Guid guid) => guid == Id;
    }

    public class Identifier<TIdentifier, TReference> {
        public TIdentifier Id { get; }
        public TReference Reference { get; }
        [JsonConstructor]
        internal Identifier(TIdentifier id, TReference reference) {
            this.Id = id;
            this.Reference = reference;
        }

        public bool Identify(TIdentifier id, Func<TIdentifier, TIdentifier, bool> comparer) => comparer(id, Id);
    }
}