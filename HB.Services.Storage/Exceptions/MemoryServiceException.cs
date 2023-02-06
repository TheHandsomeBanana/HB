using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Storage.Exceptions {
    public class MemoryServiceException : InternalException {
        public MemoryServiceException(string? message) : base(message) {
        }

        public MemoryServiceException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
