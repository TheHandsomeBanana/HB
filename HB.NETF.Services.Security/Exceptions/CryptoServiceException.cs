using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Services.Security.Exceptions {
    public class CryptoServiceException : InternalException {
        public CryptoServiceException(string message) : base(message) {
        }

        public CryptoServiceException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
