using HB.DependencyInjection.MS;
using HB.Services.Data.Handler;
using HB.Services.Data.Handler.Async;
using HB.Services.Data.Identifier;
using HB.Services.Security.Cryptography;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.DataProtection;
using HB.Services.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;

namespace HB.Package
{
    public static class HBPackage {
        //public static IServiceProvider? ServiceProvider { get; set; }

        public static void PrepareCrossPackage() {
            //DIBuilder diBuilder = new DIBuilder();
            //diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>()
            //    .AddTransient<IAsyncStreamHandler, AsyncStreamHandler>()
            //    .AddTransient<IIdentifierFactory, IdentifierFactory>()
            //    .AddTransient<IAesCryptoService, AesCryptoService>()
            //    .AddTransient<IRsaCryptoService, RsaCryptoService>()
            //    .AddTransient<ISerializerService, SerializerService>();
           
            //ServiceProvider = diBuilder.Services.BuildServiceProvider();
        }

        [SupportedOSPlatform("windows")]
        public static void PrepareWindowsPackage() {
            //DIBuilder diBuilder = new DIBuilder();
            //diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>()
            //.AddTransient<IAsyncStreamHandler, AsyncStreamHandler>()
            //.AddTransient<IIdentifierFactory, IdentifierFactory>()
            //.AddTransient<IAesCryptoService, AesCryptoService>()
            //.AddTransient<IRsaCryptoService, RsaCryptoService>()
            //.AddTransient<ISerializerService, SerializerService>()
            //.AddTransient<IDataProtectionService, DataProtectionService>();

            //ServiceProvider = diBuilder.Services.BuildServiceProvider();
        }
    }
}