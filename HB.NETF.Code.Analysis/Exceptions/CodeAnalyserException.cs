using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Exceptions {
    public class CodeAnalyserException : InternalException {
        public CodeAnalyserException(string message) : base(message) {
        }

        public CodeAnalyserException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
