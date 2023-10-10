using HB.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection.Exceptions {
    public class ServiceException : InternalException {

        public ServiceException(string? message) : base(message) {
        }
    }
}
