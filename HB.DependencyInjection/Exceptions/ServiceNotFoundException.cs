using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection.Exceptions {
    public class ServiceNotFoundException : ServiceException {
        public ServiceNotFoundException(string serviceName) : base(serviceName) {
            
        }
    }
}
