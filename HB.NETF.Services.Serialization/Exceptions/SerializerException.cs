using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Services.Serialization.Exceptions {
    public class SerializerException : InternalException {
        public SerializerException(string message) : base(message) {
        }

        public SerializerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
