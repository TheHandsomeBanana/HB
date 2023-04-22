using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Common.Serialization;
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
        public static readonly string DataModelLocation = DiscordEnvironment.CachePath + "\\" + nameof(SimplifiedDiscordDataModel) + MemoryService.MemoryExtension;
        public const SerializerMode DataSerializerMode = SerializerMode.Json;

        private ILogger logger;
        private ISimplifiedMemoryService memoryService;
        private IGenCryptoService<SimplifiedDiscordDataModel, AesKey> cryptoService;


        private DiscordDataService() {
            ILoggerFactory factory = DIContainer.ServiceProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            logger = factory.CreateLogger(nameof(DiscordDataService), b => b.WithNoTargets());
            memoryService = DIContainer.ServiceProvider.GetService(typeof(ISimplifiedMemoryService)) as ISimplifiedMemoryService;
            cryptoService = DIContainer.ServiceProvider.GetService(typeof(IGenCryptoService<SimplifiedDiscordDataModel, AesKey>)) as IGenCryptoService<SimplifiedDiscordDataModel, AesKey>;
        }

        private string token;
        private DiscordSocketClient client;
        private SimplifiedDiscordDataModel simplifiedDataModel;
        public string[] ServerFilter { get; set; }

        public DiscordDataService(string token) : this() {
            this.token = token;
            client = new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            });
        }

        public Func<Task> Ready { get; set; }

        public async Task ConnectAsync() {
            if (token == null || token == string.Empty)
                throw new TokenNotFoundException(token);

            client.Ready += client_ready;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            logger.LogInformation("Connecting.");
        }

        private Task client_ready() {
            logger.LogInformation("Connected.");
            Ready.Invoke();
            return Task.CompletedTask;
        }

        public async Task DisconnectAsync() {
            await client.LogoutAsync();
            await client.StopAsync();
            logger.LogInformation("Disconnected.");
        }

        public async Task DownloadDataAsync() {
            logger.LogInformation("Loading discord data.");

            IEnumerable<SocketGuild> servers;

            if (ServerFilter == null) {
                servers = client.Guilds;
                logger.LogWarning("No server filter set. All server data is retrieved, consider using a filter.");
            }
            else
                servers = client.Guilds.Where(e => ServerFilter.Contains(e.Name));

            List<SimplifiedDiscordServerModel> serverModels = new List<SimplifiedDiscordServerModel>();
            foreach (var server in servers) {
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

            logger.LogInformation($"{simplifiedDataModel.Servers.Length} servers downloaded.");
            if (simplifiedDataModel.Servers.Length == 0)
                return;

            await memoryService.WriteMemoryAsync(DataModelLocation, simplifiedDataModel, DataSerializerMode);
        }

        public async Task LoadFromMemoryAsync() {
            simplifiedDataModel = await memoryService.ReadMemoryAsync<SimplifiedDiscordDataModel>(DataModelLocation, DataSerializerMode);
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
