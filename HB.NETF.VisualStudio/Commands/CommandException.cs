using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.VisualStudio.Commands {
    public class CommandException : InternalException {
        public CommandException(string message) : base(message) {
        }

        public CommandException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
