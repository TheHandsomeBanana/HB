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
    public class DiscordDataServiceWrapper : IDiscordDataServiceWrapper {
        private ILogger<DiscordDataServiceWrapper> logger;
        private List<DiscordDataService> internalDiscordDataServices = new List<DiscordDataService>();
        private List<SimplifiedDiscordDataModel> simplifiedDiscordDataModels = new List<SimplifiedDiscordDataModel>();

        public DiscordDataServiceWrapper() {
            ILoggerFactory loggerFactory = DIContainer.GetService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger<DiscordDataServiceWrapper>();
        }


        public void BuildUp(params TokenModel[] tokens) {
            foreach (TokenModel token in tokens) {
                DiscordDataService service = new DiscordDataService(token);
                service.DataProcessed += DataProcessed;
                internalDiscordDataServices.Add(service);
            }

            logger.LogInformation($"Initialized with {tokens.Length} tokens.");
        }

        private Task DataProcessed() {
            logger.LogInformation($"Downloaded data through {internalDiscordDataServices.Count} tokens.");
            Dispose();
            return Task.CompletedTask;
        }

        public async Task DownloadDataAsync() {
            List<Task> downloadTasks = new List<Task>();
            foreach (DiscordDataService dataService in internalDiscordDataServices)
                downloadTasks.Add(dataService.DownloadDataAsync());

            await Task.WhenAll(downloadTasks);
        }

        public void Dispose() {
            foreach (DiscordDataService dataService in internalDiscordDataServices)
                dataService.Dispose();

            simplifiedDiscordDataModels.AddRange(internalDiscordDataServices
                .Select(e => e.SimplifiedDataModel)
                .Where(e => simplifiedDiscordDataModels.Count > 0 ?
                    e.Servers.Any(f => simplifiedDiscordDataModels.Any(g => g.Servers.Any(h => f?.Id != h?.Id))) :
                    true
                )
            );

            internalDiscordDataServices.Clear();
        }

        public DiscordItemModel[] GetServers() {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (SimplifiedDiscordDataModel dataModel in simplifiedDiscordDataModels) {
                IEnumerable<DiscordItemModel> servers = dataModel.Servers;
                if (servers == null)
                    continue;

                items.AddRange(servers.Where(e => items.Count > 0 ? items.Any(f => f?.Id != e?.Id) : true));
            }

            return items.ToArray();
        }

        public DiscordItemModel[] GetChannels(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach(SimplifiedDiscordDataModel dataModel in simplifiedDiscordDataModels) {
                IEnumerable<DiscordItemModel> channels = dataModel.GetServer(serverId)?.Channels;
                if (channels == null)
                    continue;

                items.AddRange(channels.Where(e => items.Count > 0 ? items.Any(f => f?.Id != e?.Id) : true));
            }

            return items.ToArray();
        }

        public DiscordItemModel[] GetRoles(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (SimplifiedDiscordDataModel dataModel in simplifiedDiscordDataModels) {
                IEnumerable<DiscordItemModel> roles = dataModel.GetServer(serverId)?.Roles;
                if (roles == null)
                    continue;

                items.AddRange(roles.Where(e => items.Count > 0 ? items.Any(f => f?.Id != e?.Id) : true));
            }

            return items.ToArray();
        }

        public DiscordItemModel[] GetUsers(ulong serverId) {
            List<DiscordItemModel> items = new List<DiscordItemModel>();

            foreach (SimplifiedDiscordDataModel dataModel in simplifiedDiscordDataModels) {
                IEnumerable<DiscordItemModel> users = dataModel.GetServer(serverId)?.Users;
                if(users == null)
                    continue;

                items.AddRange(users.Where(e => items.Count > 0 ? items.Any(f => f?.Id != e?.Id) : true));
            }

            return items.ToArray();
        }
    }
}
