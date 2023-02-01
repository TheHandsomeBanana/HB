using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.Services.Caching.Exceptions {
    public class CacheException : InternalException {
        public CacheException(string? message) : base(message) {
        }

        public CacheException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
