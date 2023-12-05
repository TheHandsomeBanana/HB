using Microsoft.Extensions.DependencyInjection;
using System;

namespace HB.NETF.Common.DependencyInjection {
    [Obsolete]
    public static class DIContainer {
        [Obsolete]
        public static IServiceProvider ServiceProvider { get; private set; }
        [Obsolete]
        public static void BuildServiceProvider(IServiceCollection services) {
            ServiceProvider = services.BuildServiceProvider();
        }
        [Obsolete]
        public static void BuildServiceProvider(DIBuilder builder) {
            ServiceProvider = builder.Services.BuildServiceProvider();
        }
        [Obsolete]
        public static TService GetService<TService>() {
            object service = ServiceProvider?.GetService(typeof(TService));
            if (service == null)
                return default;

            return (TService)service;
        }
    }
}
