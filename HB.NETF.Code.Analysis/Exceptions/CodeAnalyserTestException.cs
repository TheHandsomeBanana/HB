using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Code.Analysis.Exceptions {
    public class CodeAnalyserTestException : InternalException {
        public CodeAnalyserTestException(string message) : base(message) {
        }

        public CodeAnalyserTestException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
