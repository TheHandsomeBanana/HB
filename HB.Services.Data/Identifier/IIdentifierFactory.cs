using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Data.Identifier {
    public interface IIdentifierFactory {
        Identifier CreateIdentifier(object reference);
        Identifier<T> CreateIdentifier<T>(T reference);
        Identifier<TIdentifier, TReference> CreateIdentifier<TIdentifier, TReference>(TIdentifier id, TReference reference);
        bool Identify(Identifier id1, Identifier id2);
        bool Identify<T>(Identifier<T> id1, Identifier<T> id2);
        bool Identify<TIdentifier, TReference>(Identifier<TIdentifier, TReference> id1, Identifier<TIdentifier, TReference> id2, Func<TIdentifier, TIdentifier, bool> comparer);
    }
}
