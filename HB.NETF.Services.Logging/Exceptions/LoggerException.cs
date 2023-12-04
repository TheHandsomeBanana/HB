using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Services.Logging.Exceptions {
    public class LoggerException : InternalException {
        public LoggerException(string message) : base(message) {
        }

        public LoggerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
