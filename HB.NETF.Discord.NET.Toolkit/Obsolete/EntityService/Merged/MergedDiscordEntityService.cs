using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Cached;
using HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Exceptions;
using HB.NETF.Services.Logging.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Merged {
    public class MergedDiscordEntityService : IMergedDiscordEntityService {
        private List<DiscordEntityService> entityServices = new List<DiscordEntityService>();
        private readonly ILogger<MergedDiscordEntityService> logger;
        private readonly IAsyncStreamHandler streamHandler;
        public DiscordServerCollection ServerCollection { get; set; } = new DiscordServerCollection();

        public MergedDiscordEntityService() {
            this.logger = DIContainer.GetService<ILoggerFactory>().GetOrCreateLogger<MergedDiscordEntityService>();
            streamHandler = DIContainer.GetService<IAsyncStreamHandler>();
        }

        public void Init(params string[] tokens) {
            foreach (var token in tokens) {
                entityServices.Add(new DiscordEntityService(new TokenModel(nameof(MergedDiscordEntityService), token)));
            }
        }

        public void Init(params TokenModel[] tokens) {
            foreach (var token in tokens) {
                entityServices.Add(new DiscordEntityService(token));
            }
        }

        public async Task SaveMerged(string mergedPath) {
            if(entityServices.Count == 0) {
                this.logger.LogWarning($"{nameof(MergedDiscordEntityService)} is not initialized. ServerCollection not loaded.");
                return;
            }
            List<Task> connectTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                connectTasks.Add(entityService.ConnectAsync());

            await Task.WhenAll(connectTasks);

            List<Task> pullTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                pullTasks.Add(entityService.PullEntitiesAsync());

            await Task.WhenAll(pullTasks);

            foreach (DiscordEntityService entityService in entityServices) {
                foreach (DiscordServer server in entityService.GetServers()) {
                    if (!ServerCollection.ContainsKey(server.Id))
                        ServerCollection.Add(server.Id, server);
                }
            }

            List<Task> dcWriteTasks = new List<Task> {
                Task.Run(() => streamHandler.WithOptions(optionBuilder).WriteToFile<DiscordServerCollection>(mergedPath, ServerCollection))
            };

            foreach (DiscordEntityService entityService in entityServices)
                dcWriteTasks.Add(entityService.DisconnectAsync());


            await Task.WhenAll(dcWriteTasks);
            optionBuilder = null;
        }

        private OptionBuilderFunc optionBuilder;
        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }

        public void Dispose() {
            foreach (var entityService in entityServices)
                entityService.Dispose();

            entityServices.Clear();
        }

        public async Task<bool> LoadMerged(string mergedPath) {
            bool fileExists = File.Exists(mergedPath);
            if (fileExists)
                ServerCollection = await Task.Run(() => streamHandler.WithOptions(optionBuilder).ReadFromFile<DiscordServerCollection>(mergedPath));

            return fileExists;
        }
    }
}
