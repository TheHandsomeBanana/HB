using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
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

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Cached {
    public class CachedDiscordEntityService : ICachedDiscordEntityService {
        private readonly DiscordEntityService entityService;
        private readonly ILogger<CachedDiscordEntityService> logger;
        private readonly IAsyncStreamHandler streamHandler;
        private readonly TokenModel token;
        private static string cacheFile;

        public CachedDiscordEntityService(TokenModel token) {
            entityService = new DiscordEntityService(token);

            ILoggerFactory factory = DIContainer.GetService<ILoggerFactory>();
            logger = factory.GetOrCreateLogger<CachedDiscordEntityService>();

            streamHandler = DIContainer.GetService<IAsyncStreamHandler>();
            cacheFile = DiscordEnvironment.CachePath + "\\" + token.Bot + DiscordEnvironment.CacheExtension;
            this.token = token;

            if (!File.Exists(cacheFile))
                File.Create(cacheFile).Close();
        }

        public async Task<DiscordServerModel[]> GetServers()
            => entityService.GetServers() ?? (await Reload())?.GetServers();

        public async Task<DiscordChannelModel[]> GetChannels(ulong serverId)
            => entityService.GetChannels(serverId) ?? (await Reload())?.GetChannels(serverId);

        public async Task<DiscordEntityModel> GetEntity(ulong entityId) 
            => entityService.GetEntity(entityId) ?? (await Reload())?.GetEntity(entityId);

        public async Task<DiscordRoleModel[]> GetRoles(ulong serverId)
            => entityService.GetRoles(serverId) ?? (await Reload())?.GetRoles(serverId);

        public async Task<DiscordUserModel[]> GetUsers(ulong serverId)
            => entityService.GetUsers(serverId) ?? (await Reload())?.GetUsers(serverId);

        public async Task Refresh() {
            this.logger.LogInformation($"Started cache refresh. [{this.token.Bot}]");
            await entityService.ConnectAsync();
            await entityService.PullEntitiesAsync();
            Task disconnect = entityService.DisconnectAsync();
            Task write = streamHandler.WriteToFileAsync(cacheFile, entityService.DataModel);
            await Task.WhenAll(disconnect, write);
            await Reload();
            this.logger.LogInformation($"Finished cache refresh. [{this.token.Bot}]");
        }

        public void Dispose() {
            entityService.Dispose();
        }

        internal async Task<DiscordEntityService> Reload() {
            entityService.DataModel = await streamHandler.WithOptions(optionBuilder).ReadFromFileAsync<DiscordDataModel>(cacheFile);
            return entityService;
        }

        private OptionBuilderFunc optionBuilder;
        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }
    }
}
