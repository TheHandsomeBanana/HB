using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.DataProtection;
using HB.NETF.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Services.Security {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<IAesCryptoService, AesCryptoService>()
                .RegisterType<IRsaCryptoService, RsaCryptoService>()
                .RegisterType<IDataProtectionService, DataProtectionService>();
        }
    }
}
