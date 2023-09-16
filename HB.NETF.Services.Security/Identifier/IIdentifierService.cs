using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Identifier {
    public interface IIdentifierService {
        Identifier CreateIdentifier(object reference);
        Identifier<T> CreateIdentifier<T>(T reference);
        Identifier<TIdentifier, TReference> CreateIdentifier<TIdentifier, TReference>(TIdentifier id, TReference reference);
    }
}
