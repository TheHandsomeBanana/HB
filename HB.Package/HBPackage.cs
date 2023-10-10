using HB.DependencyInjection;
using HB.Services.Data.Handler;
using HB.Services.Data.Handler.Async;
using HB.Services.Data.Identifier;
using HB.Services.Security.Cryptography;
using HB.Services.Security.Cryptography.Interfaces;
using HB.Services.Security.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;

namespace HB.Package {
    public static class HBPackage {
        public static IServiceProvider? ServiceProvider { get; set; }

        public static void PreparePackage() {
            DIBuilder diBuilder = new DIBuilder();
            diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>();
            diBuilder.Services.AddTransient<IAsyncStreamHandler, AsyncStreamHandler>();
            diBuilder.Services.AddTransient<IIdentifierFactory, IdentifierFactory>();
            diBuilder.Services.AddTransient<IAesCryptoService, AesCryptoService>();
            diBuilder.Services.AddTransient<IRsaCryptoService, RsaCryptoService>();

            ServiceProvider = diBuilder.Services.BuildServiceProvider();
        }

        [SupportedOSPlatform("windows")]
        public static void PrepareWithWindowsSpecific() {
            DIBuilder diBuilder = new DIBuilder();
            diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>();
            diBuilder.Services.AddTransient<IAsyncStreamHandler, AsyncStreamHandler>();
            diBuilder.Services.AddTransient<IIdentifierFactory, IdentifierFactory>();
            diBuilder.Services.AddTransient<IAesCryptoService, AesCryptoService>();
            diBuilder.Services.AddTransient<IRsaCryptoService, RsaCryptoService>();
            diBuilder.Services.AddTransient<IDataProtectionService, DataProtectionService>();

            ServiceProvider = diBuilder.Services.BuildServiceProvider();
        }

        public static T? GetService<T>() {
            object? service = ServiceProvider?.GetService(typeof(T));
            if (service == null)
                return default;

            return (T)service;
        }
    }
}