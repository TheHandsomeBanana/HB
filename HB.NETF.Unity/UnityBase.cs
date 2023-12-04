using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Unity {
    public static class UnityBase {
        public static IUnityContainer UnityContainer { get; } = new UnityContainer();

        public static void Boot(params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(UnityContainer);
        }

        public static void Boot(IUnityContainer container, params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(container);
        }
    }
}
