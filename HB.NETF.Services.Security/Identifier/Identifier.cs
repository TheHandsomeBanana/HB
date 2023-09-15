using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Identifier {
    public class Identifier : IIdentifier {
        public Guid Identifier { get; }
        public object Reference { get; }
        public Type ReferenceType { get; }
        public Identifier(object reference, Type referenceType) {
            Identifier = Guid.NewGuid();
            Reference = reference;
            ReferenceType = referenceType;
        }

        public bool Identify(Guid guid) => guid == Identifier;
    }

    public class Identifier<TReference> : IIdentifier<TReference> {
        public Guid Identifier { get; set; }
        public TReference Reference { get; }
        public Identifier(TReference reference) {
            Identifier = Guid.NewGuid();
            Reference = reference;
        }

        public bool Identify(Guid guid) => guid == Identifier;
    }
}
