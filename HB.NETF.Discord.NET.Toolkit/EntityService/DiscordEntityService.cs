using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HB.NETF.Discord.NET.Toolkit.EntityService {
    public class DiscordEntityService : IDiscordEntityService {

        private readonly ILogger<DiscordEntityService> logger;
        private readonly DiscordSocketClient client;
        private readonly TokenModel token;
        internal DiscordDataModel DataModel { get; set; }

        // Todo: Make user connect to discord --> Retrieve Applications --> Get Bots from application
        public DiscordEntityService(TokenModel token) {
            ILoggerFactory factory = DIContainer.GetService<ILoggerFactory>();
            logger = factory.GetOrCreateLogger<DiscordEntityService>();

            client = new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            });
            this.token = token;
        }

        public bool IsReady { get; private set; }
        public async Task ConnectAsync() {
            client.Ready += Client_Ready;

            await client.LoginAsync(TokenType.Bot, token.Token);
            await client.StartAsync();
            logger.LogInformation($"[{token.Bot}] - Connecting to Discord API."); // Temporary
            Stopwatch sw = Stopwatch.StartNew();
            while (!IsReady && sw.ElapsedMilliseconds < 10000) { } // Wait for connection
            sw.Stop();
            if (!IsReady)
                logger.LogError($"[{token.Bot}] - Connection timed out.");
        }

        public async Task PullEntitiesAsync() {
            if(!IsReady) {
                logger.LogError($"[{token.Bot}] - Cannot pull entites, connection timed out.");
                return;
            }


            logger.LogInformation($"[{token.Bot}] - Loading data from Discord API."); // Temporary

            List<DiscordServerModel> serverModels = new List<DiscordServerModel>();
            foreach (var server in client.Guilds) {
                serverModels.Add(new DiscordServerModel() {
                    Id = server.Id,
                    Name = server.Name,
                    Users = await GetUsers(server),
                    Roles = GetRoles(server),
                    Channels = GetChannels(server)
                });
            }

            logger.LogInformation($"[{token.Bot}] - {serverModels.Count} servers data downloaded from Discord API."); // Temporary
            DataModel = new DiscordDataModel() { Servers = serverModels.ToArray() };
        }

        private Task Client_Ready() {
            IsReady = true;
            logger.LogInformation($"[{token.Bot}] - Connected to Discord API."); // Temporary
            return Task.CompletedTask;
        }

        public void Dispose() {
            client.Dispose();
        }


        public async Task DisconnectAsync() {
            if (!IsReady) {
                logger.LogError($"[{token.Bot}] - Cannot disconnect, connection never established.");
                return;
            }

            await client.LogoutAsync();
            await client.StopAsync();
            logger.LogInformation($"[{token.Bot}] - Disconnected from Discord API."); // Temporary
            IsReady = false;
        }

        public DiscordServerModel[] GetServers() => DataModel?.Servers;
        public DiscordUserModel[] GetUsers(ulong serverId) => DataModel?.GetServer(serverId)?.Users;
        public DiscordRoleModel[] GetRoles(ulong serverId) => DataModel?.GetServer(serverId)?.Roles;
        public DiscordChannelModel[] GetChannels(ulong serverId) => DataModel?.GetServer(serverId)?.Channels;
        public DiscordEntityModel GetEntity(ulong entityId) => DataModel.GetEntity(entityId);

        #region Helper 
        private async Task<DiscordUserModel[]> GetUsers(SocketGuild guild) {
            await guild.DownloadUsersAsync();
            return guild.Users.Select(f => new DiscordUserModel { Id = f.Id, Name = f.IsBot ? $"{f.Username} [BOT]" : f.Username }).ToArray();
        }

        private DiscordRoleModel[] GetRoles(SocketGuild guild) {
            return guild.Roles.Select(e => new DiscordRoleModel { Id = e.Id, Name = e.Name }).ToArray();
        }

        private DiscordChannelModel[] GetChannels(SocketGuild guild) {
            return guild.Channels.Select(e => new DiscordChannelModel { Id = e.Id, Name = e.Name }).ToArray();
        }
        #endregion
    }
}
