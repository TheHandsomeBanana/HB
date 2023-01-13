using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.DependencyInjection.Exceptions {
    public class DependencyException : InternalException {
        public DependencyException(string? message) : base(message) {
        }

        public DependencyException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
