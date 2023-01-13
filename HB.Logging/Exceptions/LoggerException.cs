using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Logging.Exceptions {
    public class LoggerException : InternalException {
        public LoggerException(string? message) : base(message) {
        }

        public LoggerException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
