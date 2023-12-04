using System;

namespace HB.NETF.Services.Serialization {
    public enum SerializerMode {
        Json,
        Xml,
        [Obsolete("Usage of binary formatting is dangerous and should not be used.")]
        Binary
    }
}
