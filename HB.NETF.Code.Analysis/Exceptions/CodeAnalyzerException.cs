using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Code.Analysis.Exceptions {
    public class CodeAnalyzerException : InternalException {
        public CodeAnalyzerException(string message) : base(message) {
        }

        public CodeAnalyzerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
