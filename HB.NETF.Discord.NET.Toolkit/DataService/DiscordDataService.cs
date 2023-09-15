using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Common.Serialization;
using HB.NETF.Discord.NET.Toolkit.DataService.Models;
using HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService {
    public class DiscordDataService : IDiscordDataService {
        public string DataModelLocation { get; }
        public const SerializerMode DataSerializerMode = SerializerMode.Json;

        private ILogger<DiscordDataService> logger;
        private ISimplifiedSerializerService memoryService;
        private IGenCryptoService<SimplifiedDiscordDataModel, AesKey> cryptoService;


        private DiscordDataService() {
            ILoggerFactory factory = DIContainer.GetService<ILoggerFactory>();
            logger = factory.CreateLogger<DiscordDataService>();
            memoryService = DIContainer.GetService<ISimplifiedSerializerService>();
            cryptoService = DIContainer.GetService<IGenCryptoService<SimplifiedDiscordDataModel, AesKey>>();
        }

        private TokenModel token;
        private DiscordSocketClient client;
        internal SimplifiedDiscordDataModel SimplifiedDataModel { get; set; } = new SimplifiedDiscordDataModel();

        public DiscordDataService(TokenModel token) : this() {
            this.token = token;
            DataModelLocation = DiscordEnvironment.CachePath + "\\" + token.Bot + "_" + nameof(SimplifiedDiscordDataModel) + SerializerService.MemoryExtension;
            
            client = new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            });
        }

        public async Task DownloadDataAsync() {
            if (token == null || token.Token == string.Empty)
                throw new TokenNotFoundException();

            client.Ready += client_ready;
            await client.LoginAsync(TokenType.Bot, token.Token);
            await client.StartAsync();
            logger.LogInformation($"Connecting to Discord API [{token.Bot}].");
        } 

        public async Task<DiscordItemModel[]> GetServers() {
            if (SimplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return SimplifiedDataModel.Servers;
        }

        public async Task<DiscordItemModel[]> GetChannels(ulong serverId) {
            if (SimplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return SimplifiedDataModel.GetServer(serverId)?.Channels;
        }

        public async Task<DiscordItemModel[]> GetRoles(ulong serverId) {
            if (SimplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return SimplifiedDataModel.GetServer(serverId)?.Roles;
        }

        public async Task<DiscordItemModel[]> GetUsers(ulong serverId) {
            if (SimplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return SimplifiedDataModel.GetServer(serverId)?.Users;
        }

        public void Dispose() {
            client.Dispose();
        }

        internal Func<Task> DataProcessed { get; set; }

        private async Task client_ready() {
            logger.LogInformation($"Connected to Discord API [{token.Bot}].");
            await DownloadDataInternalAsync();
        }

        private async Task DownloadDataInternalAsync() {
            logger.LogInformation($"Loading data from Discord API [{token.Bot}.]"); 

            List<SimplifiedDiscordServerModel> serverModels = new List<SimplifiedDiscordServerModel>();
            foreach (var server in client.Guilds) {
                serverModels.Add(new SimplifiedDiscordServerModel() {
                    Id = server.Id,
                    Name = server.Name,
                    ItemModelType = DiscordItemModelType.Server,
                    Users = await GetUsers(server),
                    Roles = GetRoles(server),
                    Channels = GetChannels(server)
                });
            }

            SimplifiedDataModel.Servers = serverModels.ToArray();

            logger.LogInformation($"{SimplifiedDataModel.Servers.Length} servers data downloaded from Discord API [{token.Bot}].");
            if (SimplifiedDataModel.Servers.Length == 0)
                return;

            await memoryService.WriteAsync(DataModelLocation, SimplifiedDataModel, DataSerializerMode);
            await DisconnectAsync();
        }

        private async Task DisconnectAsync() {
            await client.LogoutAsync();
            await client.StopAsync();
            logger.LogInformation($"Disconnected from Discord API [{token.Bot}].");
            await DataProcessed.Invoke();
        }

        private async Task LoadFromMemoryAsync() {
            SimplifiedDataModel = await memoryService.ReadAsync<SimplifiedDiscordDataModel>(DataModelLocation, DataSerializerMode);
        }

        #region Helper 
        private async Task<DiscordItemModel[]> GetUsers(SocketGuild guild) {
            await guild.DownloadUsersAsync();
            return guild.Users.Select(f => new DiscordItemModel { Id = f.Id, Name = f.IsBot ? $"{f.Username} [BOT]" : f.Username, ItemModelType = DiscordItemModelType.User }).ToArray();
        }

        private DiscordItemModel[] GetRoles(SocketGuild guild) {
            return guild.Roles.Select(e => new DiscordItemModel { Id = e.Id, Name = e.Name, ItemModelType = DiscordItemModelType.Role }).ToArray();
        }

        private DiscordItemModel[] GetChannels(SocketGuild guild) {
            return guild.Channels.Select(e => new DiscordItemModel { Id = e.Id, Name = e.Name, ItemModelType = DiscordItemModelType.Channel }).ToArray();
        }
        #endregion
    }
}
