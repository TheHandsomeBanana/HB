using HB.NETF.Common.DependencyInjection;
using HB.NETF.Common.Serialization;
using HB.NETF.Discord.NET.Toolkit.DataService.Models;
using HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService {
    internal class ExtendedDiscordDataService : IExtendedDiscordDataService {
        private ILogger<ExtendedDiscordDataService> logger;
        private List<DiscordDataService> internalDiscordDataServices = new List<DiscordDataService>();

        public ExtendedDiscordDataService() {
            ILoggerFactory loggerFactory = DIContainer.GetService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger<ExtendedDiscordDataService>();
        }


        public void BuildUp(params TokenModel[] tokens) {
            foreach (TokenModel token in tokens)
                internalDiscordDataServices.Add(new DiscordDataService(token));

            logger.LogInformation($"Initialized with {tokens.Length} tokens.");
        }

        public async Task DownloadDataAsync() {
            foreach (DiscordDataService dataService in internalDiscordDataServices)
                await dataService.DownloadDataAsync();

            logger.LogInformation($"Downloaded data for {internalDiscordDataServices.Count} tokens.");
        }

        public void Dispose() {
            foreach (DiscordDataService dataService in internalDiscordDataServices)
                dataService.Dispose();
        }

        public async Task<DiscordItemModel[]> GetChannels(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (DiscordDataService dataService in internalDiscordDataServices) {
                DiscordItemModel[] channels = await dataService.GetChannels(serverId);
                items.AddRange(channels.Where(e => items.Any(f => f.Id != e.Id)));
            }

            return items.ToArray();
        }

        public async Task<DiscordItemModel[]> GetRoles(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (DiscordDataService dataService in internalDiscordDataServices) {
                DiscordItemModel[] roles = await dataService.GetRoles(serverId);
                items.AddRange(roles.Where(e => items.Any(f => f.Id != e.Id)));
            }

            return items.ToArray();
        }

        public async Task<DiscordItemModel[]> GetServers() {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (DiscordDataService dataService in internalDiscordDataServices) {
                DiscordItemModel[] servers = await dataService.GetServers();
                items.AddRange(servers.Where(e => items.Any(f => f.Id != e.Id)));
            }

            return items.ToArray();
        }

        public async Task<DiscordItemModel[]> GetUsers(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (DiscordDataService dataService in internalDiscordDataServices) {
                DiscordItemModel[] users = await dataService.GetUsers(serverId);
                items.AddRange(users.Where(e => items.Any(f => f.Id != e.Id)));
            }

            return items.ToArray();
        }
    }
}
