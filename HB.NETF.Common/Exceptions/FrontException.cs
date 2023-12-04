using System;
using System.Runtime.Serialization;

namespace HB.NETF.Common.Exceptions {
    public class FrontException : Exception {
        public FrontException() {
        }

        public FrontException(string message) : base(message) {
        }

        public FrontException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FrontException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
