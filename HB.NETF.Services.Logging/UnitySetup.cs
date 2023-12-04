using HB.NETF.Services.Logging.Factory;
using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Services.Logging {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterSingleton<ILoggerFactory, LoggerFactory>();
        }
    }
}
