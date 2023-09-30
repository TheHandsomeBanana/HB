using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService;
using HB.NETF.Discord.NET.Toolkit.EntityService.Cached;
using HB.NETF.Discord.NET.Toolkit.EntityService.Cached.Handler;
using HB.NETF.Discord.NET.Toolkit.EntityService.Handler;
using HB.NETF.Discord.NET.Toolkit.EntityService.Merged;
using HB.NETF.Discord.NET.Toolkit.TokenService;
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
    public static class HBPackage {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void PreparePackage() {
            DIBuilder diBuilder = new DIBuilder();
            diBuilder.Services.AddTransient<IStreamHandler, StreamHandler>();
            diBuilder.Services.AddTransient<IAsyncStreamHandler, AsyncStreamHandler>();
            diBuilder.Services.AddTransient<IIdentifierFactory, IdentifierFactory>();
            diBuilder.Services.AddTransient<IAesCryptoService, AesCryptoService>();
            diBuilder.Services.AddTransient<IRsaCryptoService, RsaCryptoService>();
            diBuilder.Services.AddTransient<IDataProtectionService, DataProtectionService>();
            diBuilder.Services.AddSingleton<IDiscordEntityService, DiscordEntityService>();
            diBuilder.Services.AddSingleton<ICachedDiscordEntityService, CachedDiscordEntityService>();
            diBuilder.Services.AddSingleton<IDiscordEntityServiceHandler, DiscordEntityServiceHandler>();
            diBuilder.Services.AddSingleton<ICachedDiscordEntityServiceHandler, CachedDiscordEntityServiceHandler>();
            diBuilder.Services.AddSingleton<IDiscordTokenService, DiscordTokenService>();
            diBuilder.Services.AddSingleton<IMergedDiscordEntityService, MergedDiscordEntityService>();

            ServiceProvider = diBuilder.Services.BuildServiceProvider();
        }

        public static T GetService<T>() {
            object service = ServiceProvider?.GetService(typeof(T));
            if (service == null)
                return default;

            return (T)service;
        }
    }
}
