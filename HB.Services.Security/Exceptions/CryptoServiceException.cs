using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Security.Exceptions {
    public class CryptoServiceException : InternalException {
        public CryptoServiceException(string? message) : base(message) {
        }

        public CryptoServiceException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
