using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Security.Exceptions {
    public class StreamWrapperException : InternalException {
        public StreamWrapperException(string message) : base(message) {
        }

        public StreamWrapperException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
