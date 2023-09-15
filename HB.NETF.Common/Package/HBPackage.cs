using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.Package {
    public static class HBPackage {
        private static IServiceProvider serviceProvider;

        public static void PreparePackage() {

        }

        public static object GetService(Type serviceType) => serviceProvider.GetService(serviceType);
        public static T GetService<T>() where T : class => serviceProvider.GetService(typeof(T)) as T ?? default(T);
    }
}
