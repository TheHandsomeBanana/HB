using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Unity {
    public static class UnityBoot {
        public static IUnityContainer UnityContainer => UnityBase.UnityContainer;
        public static void Boot(params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(UnityContainer);
        }
    }
}
