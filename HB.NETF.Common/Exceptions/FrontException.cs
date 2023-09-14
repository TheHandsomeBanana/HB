using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.Exceptions {
    public class FrontException : Exception {
        public FrontException() {
        }

        public FrontException(string message) : base(message) {
        }

        public FrontException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FrontException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
