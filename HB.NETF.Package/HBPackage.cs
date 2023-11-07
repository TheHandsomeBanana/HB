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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Package {
    /// <summary>
    /// Use for all packages provided by HB. 
    /// <br>Uses <see cref="DIContainer"/> -> overrides previous dependencies.</br>
    /// </summary>
    public static class HBPackage {
        public static void PreparePackage() {
            DIBuilder diBuilder = new DIBuilder();
            diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>();
            diBuilder.Services.AddTransient<IAsyncStreamHandler, AsyncStreamHandler>();
            diBuilder.Services.AddTransient<IIdentifierFactory, IdentifierFactory>();
            diBuilder.Services.AddTransient<IAesCryptoService, AesCryptoService>();
            diBuilder.Services.AddTransient<IRsaCryptoService, RsaCryptoService>();
            diBuilder.Services.AddTransient<IDataProtectionService, DataProtectionService>();
            diBuilder.Services.AddSingleton<IAnalyserFactory, AnalyserFactory>();

            DIContainer.BuildServiceProvider(diBuilder);
        }

        public static T GetService<T>() => DIContainer.GetService<T>();
    }
}
