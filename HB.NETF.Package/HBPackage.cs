using HB.NETF.Code.Analysis.Factory;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Identifier;
using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.DataProtection;
using HB.NETF.Unity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Package {
    /// <summary>
    /// Use for all packages provided by HB. 
    /// <br>Uses <see cref="DIContainer"/> -> overrides previous dependencies.</br>
    /// </summary>
    public static class HBPackage {
        public static IUnityContainer UnityContainer { get; private set; }
        public static void PreparePackage() {
            UnityContainer = UnityBase.UnityContainer.CreateChildContainer();
            UnityBase.Boot(UnityContainer, new UnitySetup());
        }

        public static T GetService<T>() => UnityContainer.Resolve<T>();
    }
}
