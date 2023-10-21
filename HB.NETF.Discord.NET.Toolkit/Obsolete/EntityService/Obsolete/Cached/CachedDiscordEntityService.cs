using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Handler.Options;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Obsolete.Cached {
    [Obsolete]
    public class CachedDiscordEntityService : ICachedDiscordEntityService {
        private readonly DiscordEntityService entityService;
        private readonly ILogger<CachedDiscordEntityService> logger;
        private readonly IAsyncStreamHandler streamHandler;
        private readonly TokenModel token;
        public string CacheFile { get; set; }

        public CachedDiscordEntityService(TokenModel token) {
            entityService = new DiscordEntityService(token);

            ILoggerFactory factory = DIContainer.GetService<ILoggerFactory>();
            logger = factory.GetOrCreateLogger<CachedDiscordEntityService>();

            streamHandler = DIContainer.GetService<IAsyncStreamHandler>();
            CacheFile = DiscordEnvironment.CachePath + "\\" + token.Bot + DiscordEnvironment.CacheExtension;
            this.token = token;

            if (!File.Exists(CacheFile))
                File.Create(CacheFile).Close();
        }

        public async Task<DiscordServer[]> GetServers()
            => entityService.GetServers() ?? (await Reload())?.GetServers();

        public async Task<DiscordChannel[]> GetChannels(ulong serverId)
            => entityService.GetChannels(serverId) ?? (await Reload())?.GetChannels(serverId);

        public async Task<DiscordEntity> GetEntity(ulong entityId)
            => entityService.GetEntity(entityId) ?? (await Reload())?.GetEntity(entityId);

        public async Task<DiscordRole[]> GetRoles(ulong serverId)
            => entityService.GetRoles(serverId) ?? (await Reload())?.GetRoles(serverId);

        public async Task<DiscordUser[]> GetUsers(ulong serverId)
            => entityService.GetUsers(serverId) ?? (await Reload())?.GetUsers(serverId);

        public async Task Refresh() {
            this.logger.LogInformation($"Started cache refresh. [{this.token.Bot}]");
            await entityService.ConnectAsync();
            await entityService.PullEntitiesAsync();
            Task disconnect = entityService.DisconnectAsync();
            Task write = streamHandler.WriteToFileAsync(CacheFile, entityService.ServerCollection);
            await Task.WhenAll(disconnect, write);
            await Reload();
            this.logger.LogInformation($"Finished cache refresh. [{this.token.Bot}]");
        }

        public void Dispose() {
            entityService.Dispose();
        }

        internal async Task<DiscordEntityService> Reload() {
            entityService.ServerCollection = await streamHandler.WithOptions(optionBuilder).ReadFromFileAsync<DiscordServerCollection>(CacheFile);
            return entityService;
        }

        private OptionBuilderFunc optionBuilder;
        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }
    }
}
