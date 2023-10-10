using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Identifier {
    public class IdentifierFactory : IIdentifierFactory {
        public Identifier CreateIdentifier(object reference) => new Identifier(reference);
        public Identifier<T> CreateIdentifier<T>(T reference) => new Identifier<T>(reference);

        public Identifier<TIdentifier, TReference> CreateIdentifier<TIdentifier, TReference>(TIdentifier id, TReference reference)
            => new Identifier<TIdentifier, TReference>(id, reference);

        public bool Identify(Identifier id1, Identifier id2) => id1.Identify(id2.Id);
        public bool Identify<T>(Identifier<T> id1, Identifier<T> id2) => id1.Identify(id2.Id);
        public bool Identify<TIdentifier, TReference>(Identifier<TIdentifier, TReference> id1, Identifier<TIdentifier, TReference> id2, Func<TIdentifier, TIdentifier, bool> comparer)
            => id1.Identify(id2.Id, comparer);
    }
}
