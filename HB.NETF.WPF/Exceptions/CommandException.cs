using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.WPF.Exceptions {
    public class CommandException : FrontException {
        public CommandException(Exception innerException) : base(null, innerException) {
        }
        public CommandException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
