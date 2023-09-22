using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Data.Exceptions {
    public class StreamHandlerException : InternalException {
        public StreamHandlerException(string message) : base(message) {
        }

        public StreamHandlerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
