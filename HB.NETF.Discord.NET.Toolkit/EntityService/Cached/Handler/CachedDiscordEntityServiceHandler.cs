using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Handler;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Handler.Options;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Cached.Handler {
    public class CachedDiscordEntityServiceHandler : ICachedDiscordEntityServiceHandler {
        private readonly List<CachedDiscordEntityService> entityServices = new List<CachedDiscordEntityService>();
        private readonly ILogger<CachedDiscordEntityServiceHandler> logger;
        private readonly DiscordServerCollection ServerCollection = new DiscordServerCollection();
        public CachedDiscordEntityServiceHandler() {
            ILoggerFactory loggerFactory = DIContainer.GetService<ILoggerFactory>();
            logger = loggerFactory.GetOrCreateLogger<CachedDiscordEntityServiceHandler>();
        }

        public void Init(params TokenModel[] tokens) {
            foreach (TokenModel token in tokens) {
                CachedDiscordEntityService entityService = new CachedDiscordEntityService(token);
                entityServices.Add(entityService);
            }

            ReloadServerCollection().Wait();
        }

        public DiscordServerModel[] GetServers() => ServerCollection.Values.ToArray();
        public DiscordChannelModel[] GetChannels(ulong serverId) => ServerCollection.GetChannels(serverId);
        public DiscordUserModel[] GetUsers(ulong serverId) => ServerCollection.GetUsers(serverId);
        public DiscordRoleModel[] GetRoles(ulong serverId) => ServerCollection.GetRoles(serverId);
        public DiscordEntityModel GetEntity(ulong entityId) => ServerCollection.GetEntity(entityId);

        public async Task Refresh() {
            List<Task> refreshTasks = new List<Task>();
            foreach (CachedDiscordEntityService entityService in entityServices)
                refreshTasks.Add(entityService.Refresh());

            await Task.WhenAll(refreshTasks);
            await ReloadServerCollection();
        }

        public void Dispose() {
            foreach (CachedDiscordEntityService dataService in entityServices)
                dataService.Dispose();

            entityServices.Clear();
        }

        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            foreach (CachedDiscordEntityService entityService in entityServices)
                entityService.ManipulateStream(optionBuilder);
        }

        private async Task ReloadServerCollection() {
            foreach (CachedDiscordEntityService entityService in entityServices) {
                foreach (DiscordServerModel server in await entityService.GetServers()) {
                    if (!ServerCollection.ContainsKey(server.Id))
                        ServerCollection.Add(server.Id, server);
                }
            }
        }
    }
}
