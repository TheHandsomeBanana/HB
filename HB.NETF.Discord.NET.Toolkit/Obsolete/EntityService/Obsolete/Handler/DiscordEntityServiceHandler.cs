using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Obsolete.Handler {
    [Obsolete]
    public class DiscordEntityServiceHandler : IDiscordEntityServiceHandler {
        private ILogger<DiscordEntityServiceHandler> logger;
        private List<DiscordEntityService> entityServices = new List<DiscordEntityService>();

        public DiscordServerCollection ServerCollection = new DiscordServerCollection();

        public DiscordEntityServiceHandler() {
            ILoggerFactory loggerFactory = DIContainer.GetService<ILoggerFactory>();
            logger = loggerFactory.GetOrCreateLogger<DiscordEntityServiceHandler>();
        }

        public void Init(params TokenModel[] tokens) {
            foreach (TokenModel token in tokens) {
                DiscordEntityService service = new DiscordEntityService(token);
                entityServices.Add(service);
            }

            logger.LogInformation($"Initialized with {tokens.Length} tokens.");
        }

        public async Task ConnectAsync() {
            List<Task> connectTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                connectTasks.Add(entityService.ConnectAsync());

            await Task.WhenAll(connectTasks);
        }

        public async Task PullEntitiesAsync() {
            List<Task> downloadTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                downloadTasks.Add(entityService.PullEntitiesAsync());

            await Task.WhenAll(downloadTasks);

            foreach (DiscordEntityService entityService in entityServices) {
                foreach (DiscordServer server in entityService.GetServers()) {
                    if (!ServerCollection.ContainsKey(server.Id))
                        ServerCollection.Add(server.Id, server);
                }
            }
        }

        public async Task DisconnectAsync() {
            List<Task> disconnectTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                disconnectTasks.Add(entityService.DisconnectAsync());

            await Task.WhenAll(disconnectTasks);
        }

        public void Dispose() {
            foreach (DiscordEntityService entityService in entityServices)
                entityService.Dispose();

            entityServices.Clear();
        }

        public DiscordServer[] GetServers() => ServerCollection.Values.ToArray();
        public DiscordUser[] GetUsers(ulong serverId) => ServerCollection.GetUsers(serverId);
        public DiscordRole[] GetRoles(ulong serverId) => ServerCollection.GetRoles(serverId);
        public DiscordChannel[] GetChannels(ulong serverId) => ServerCollection.GetChannels(serverId);
        public DiscordEntity GetEntity(ulong entityId) => ServerCollection.GetEntity(entityId);
    }
}
