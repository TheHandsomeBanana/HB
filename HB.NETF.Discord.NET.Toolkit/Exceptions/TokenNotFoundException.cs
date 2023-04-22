using HB.NETF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Exceptions {
    public class TokenNotFoundException : InternalException {
        public TokenNotFoundException(string message) : base(message) {
            
        }
    }
}
