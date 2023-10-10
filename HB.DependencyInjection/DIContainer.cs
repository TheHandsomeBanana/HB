using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection {
    public static class DIContainer {      
        
        public static IServiceProvider? ServiceProvider { get; private set; }

        public static void BuildServiceProvider(IServiceCollection services) {
            ServiceProvider = services.BuildServiceProvider();
        }

        public static void BuildServiceProvider(DIBuilder builder) { 
            ServiceProvider = builder.Services.BuildServiceProvider();
        }

        public static TService? GetService<TService>() {
            object? service = ServiceProvider?.GetService(typeof(TService));
            if (service == null)
                return default;

            return (TService)service;
        }
    }
}
