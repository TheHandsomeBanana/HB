using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.WPF.Exceptions {
    public class CommandException : FrontException {
        public CommandException(Exception innerException) : base(null, innerException) {
        }
        public CommandException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
