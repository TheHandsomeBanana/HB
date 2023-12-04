using HB.NETF.Code.Analysis.Factory;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Identifier;
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

namespace HB.NETF.Package {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<IStreamHandler, StreamHandler>()
                .RegisterType<IAsyncStreamHandler, AsyncStreamHandler>()
                .RegisterType<IAesCryptoService, AesCryptoService>()
                .RegisterType<IRsaCryptoService, RsaCryptoService>()
                .RegisterType<IDataProtectionService, DataProtectionService>()
                .RegisterType<IAnalyserFactory, AnalyserFactory>()
                .RegisterType<IIdentifierFactory, IdentifierFactory>();
        }
    }
}
