using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Serialization {
    public enum SerializerMode {
        None,
        Json,
        Xml,
        [Obsolete("Usage of binary formatting is dangerous and should not be used.")]
        Binary
    }
}
