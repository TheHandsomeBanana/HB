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
        private ISimplifiedMemoryService memoryService;
        private IGenCryptoService<SimplifiedDiscordDataModel, AesKey> cryptoService;


        private DiscordDataService() {
            ILoggerFactory factory = DIContainer.GetService<ILoggerFactory>();
            logger = factory.CreateLogger<DiscordDataService>();
            memoryService = DIContainer.GetService<ISimplifiedMemoryService>();
            cryptoService = DIContainer.GetService<IGenCryptoService<SimplifiedDiscordDataModel, AesKey>>();
        }

        private TokenModel token;
        private DiscordSocketClient client;
        private SimplifiedDiscordDataModel simplifiedDataModel;

        public DiscordDataService(TokenModel token) : this() {
            this.token = token;
            DataModelLocation = DiscordEnvironment.CachePath + "\\" + token.Bot + token.CreatedOn.ToString("yyyyMMdd_hhmmss") + "\\" + nameof(SimplifiedDiscordDataModel) + MemoryService.MemoryExtension;
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
            logger.LogInformation($"Connecting with {token.Bot}");
        } 

        public async Task<DiscordItemModel[]> GetServers() {
            if (simplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return simplifiedDataModel.Servers;
        }

        public async Task<DiscordItemModel[]> GetChannels(ulong serverId) {
            if (simplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return simplifiedDataModel.GetServer(serverId).Channels;
        }

        public async Task<DiscordItemModel[]> GetRoles(ulong serverId) {
            if (simplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return simplifiedDataModel.GetServer(serverId).Roles;
        }

        public async Task<DiscordItemModel[]> GetUsers(ulong serverId) {
            if (simplifiedDataModel == null)
                await LoadFromMemoryAsync();

            return simplifiedDataModel.GetServer(serverId).Users;
        }

        public void Dispose() {
            client.Dispose();
        }

        private async Task client_ready() {
            logger.LogInformation($"Connected with {token.Bot}.");
            await DownloadDataInternalAsync();
        }

        private async Task DownloadDataInternalAsync() {
            logger.LogInformation($"Loading discord data with {token.Bot}"); 

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

            simplifiedDataModel.Servers = serverModels.ToArray();

            logger.LogInformation($"{simplifiedDataModel.Servers.Length} servers downloaded with {token.Bot}.");
            if (simplifiedDataModel.Servers.Length == 0)
                return;

            await memoryService.WriteMemoryAsync(DataModelLocation, simplifiedDataModel, DataSerializerMode);
        }

        private async Task DisconnectAsync() {
            await client.LogoutAsync();
            await client.StopAsync();
            logger.LogInformation($"Disconnected with {token.Bot}");
        }

        private async Task LoadFromMemoryAsync() {
            simplifiedDataModel = await memoryService.ReadMemoryAsync<SimplifiedDiscordDataModel>(DataModelLocation, DataSerializerMode);
        }

        #region Helper 
        private async Task<DiscordItemModel[]> GetUsers(SocketGuild guild) {
            await guild.DownloadUsersAsync();
            return guild.Users.Select(f => new DiscordItemModel { Id = f.Id, Name = f.Username, ItemModelType = DiscordItemModelType.User }).ToArray();
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
