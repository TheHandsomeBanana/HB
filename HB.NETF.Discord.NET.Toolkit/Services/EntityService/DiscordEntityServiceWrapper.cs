using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public class DiscordEntityServiceWrapper : IDiscordEntityService {
        private readonly IAsyncStreamHandler streamHandler;
        private readonly List<DiscordEntityService> entityServices = new List<DiscordEntityService>();
        public DiscordServerCollection ServerCollection { get; private set; }
        public DiscordEntityServiceWrapper() {
            streamHandler = DIContainer.GetService<IAsyncStreamHandler>();
        }

        public void Init(params string[] tokens) {
            foreach (string token in tokens) {
                DiscordEntityService entityService = new DiscordEntityService();
                entityService.Init(token);
                entityServices.Add(entityService);
            }
        }

        public DiscordChannel[] GetChannels(ulong serverId) => ServerCollection.GetChannels(serverId);
        public DiscordEntity GetEntity(ulong entityId) => ServerCollection.GetEntity(entityId);
        public DiscordRole[] GetRoles(ulong serverId) => ServerCollection.GetRoles(serverId);
        public DiscordServer[] GetServers() => ServerCollection.GetServers();
        public DiscordUser[] GetUsers(ulong serverId) => ServerCollection.GetUsers(serverId);

        public async Task LoadEntities() {
            List<Task> loadTasks = new List<Task>();
            foreach (DiscordEntityService entityService in entityServices)
                loadTasks.Add(entityService.LoadEntities());


            await Task.WhenAll(loadTasks);

            foreach (DiscordEntityService entityService in entityServices) {
                foreach (DiscordServer server in entityService.GetServers()) {
                    if (!ServerCollection.ContainsKey(server.Id))
                        ServerCollection.Add(server.Id, server);
                }
            }
        }

        public async Task SaveToFile(string fileName) {
            await Task.Run(() => streamHandler.WithOptions(optionBuilder).WriteToFile<DiscordServerCollection>(fileName, ServerCollection));
        }

        public void Dispose() {
            foreach(DiscordEntityService service in entityServices) {
                service.Dispose();
            }
        }

        public async ValueTask DisposeAsync() {
            foreach(DiscordEntityService service in entityServices) 
                await service.DisposeAsync();
        }

        public async Task<bool> ReadFromFile(string fileName) {
            bool fileExists = File.Exists(fileName);
            if (fileExists)
                ServerCollection = await Task.Run(() => streamHandler.WithOptions(optionBuilder).ReadFromFile<DiscordServerCollection>(fileName));

            return fileExists;
        }

        private OptionBuilderFunc optionBuilder;
        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }
    }
}
