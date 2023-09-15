using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Identifier {
    public class Identifier : IIdentifier {
        public Guid Id { get; }
        public object Reference { get; }
        public Type ReferenceType { get; }
        public Identifier(object reference, Type referenceType) {
            Id = Guid.NewGuid();
            Reference = reference;
            ReferenceType = referenceType;
        }

        public bool Identify(Guid guid) => guid == Id;
    }

    public class Identifier<TReference> : IIdentifier<TReference> {
        public Guid Id { get; set; }
        public TReference Reference { get; }
        public Identifier(TReference reference) {
            Id = Guid.NewGuid();
            Reference = reference;
        }

        public bool Identify(Guid guid) => guid == Id;
    }
}
