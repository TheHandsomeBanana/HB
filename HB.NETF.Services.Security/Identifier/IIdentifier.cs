using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Identifier {
    public interface IIdentifier {
        bool Identify(Guid guid);
        object Reference { get; }
        Type ReferenceType { get; }
    }

    public interface IIdentifier<TReference> {
        bool Identify(Guid identifier);
        TReference Reference { get; }
    }
}
