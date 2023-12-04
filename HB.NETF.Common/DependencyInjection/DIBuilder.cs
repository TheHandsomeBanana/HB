using Microsoft.Extensions.DependencyInjection;

namespace HB.NETF.Common.DependencyInjection {
    public class DIBuilder {
        public ServiceCollection Services { get; } = new ServiceCollection();
    }
}
