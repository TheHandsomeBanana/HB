using Microsoft.Extensions.DependencyInjection;
using System;

namespace HB.NETF.Common.DependencyInjection {
    [Obsolete]
    public class DIBuilder {
        [Obsolete]
        public ServiceCollection Services { get; } = new ServiceCollection();
    }
}
