using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Unity {
    public static class UnityBase {
        public static IUnityContainer UnityContainer { get; } = new UnityContainer();
    }
}
