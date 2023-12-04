using HB.NETF.Common.TimeTracker;
using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Common {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<ITimeTracker, TimeTracker.TimeTracker>();
        }
    }
}
