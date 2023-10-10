using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services.Serialization.Exceptions {
    public class SerializerException : InternalException {
        public SerializerException(string message) : base(message) {
        }

        public SerializerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
