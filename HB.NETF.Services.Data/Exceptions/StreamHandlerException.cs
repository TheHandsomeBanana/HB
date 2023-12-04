using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Services.Data.Exceptions {
    public class StreamHandlerException : InternalException {
        public StreamHandlerException(string message) : base(message) {
        }

        public StreamHandlerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
