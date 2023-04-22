using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.DependencyInjection {
    public class DIContainer {        
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void BuildServiceProvider(IServiceCollection services) {
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
