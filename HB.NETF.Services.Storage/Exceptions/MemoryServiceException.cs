using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Services.Storage.Exceptions {
    public class MemoryServiceException : InternalException {
        public MemoryServiceException(string message) : base(message) {
        }

        public MemoryServiceException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
