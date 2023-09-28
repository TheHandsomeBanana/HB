using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
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
using System.Xml.Linq;

namespace HB.NETF.Discord.NET.Toolkit.EntityService {
    public class DiscordEntityService : IDiscordEntityService {

        private readonly ILogger<DiscordEntityService> logger;
        private readonly DiscordSocketClient client;
        private readonly TokenModel token;
        internal DiscordServerCollection ServerCollection { get; set; }

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
            if (!IsReady) {
                logger.LogError($"[{token.Bot}] - Cannot pull entites, connection timed out.");
                return;
            }


            logger.LogInformation($"[{token.Bot}] - Loading data from Discord API."); // Temporary

            List<DiscordServer> servers = new List<DiscordServer>();
            foreach (var server in client.Guilds) {
                servers.Add(new DiscordServer() {
                    Id = server.Id,
                    Name = server.Name,
                    UserCollection = await GetUsers(server),
                    RoleCollection = GetRoles(server),
                    ChannelCollection = GetChannels(server)
                });
            }

            logger.LogInformation($"[{token.Bot}] - {servers.Count} servers data downloaded from Discord API."); // Temporary
            ServerCollection = new DiscordServerCollection(servers);
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

        public DiscordServer[] GetServers() => ServerCollection?.GetServers();
        public DiscordUser[] GetUsers(ulong serverId) => ServerCollection?.GetUsers(serverId);
        public DiscordRole[] GetRoles(ulong serverId) => ServerCollection?.GetRoles(serverId);
        public DiscordChannel[] GetChannels(ulong serverId) => ServerCollection?.GetChannels(serverId);
        public DiscordEntity GetEntity(ulong entityId) => ServerCollection?.GetEntity(entityId);

        #region Helper 
        private async Task<Dictionary<ulong, DiscordUser>> GetUsers(SocketGuild guild) {
            await guild.DownloadUsersAsync();

            return guild.Users.ToDictionary(e => e.Id, e => new DiscordUser { Id = e.Id, Name = e.IsBot ? $"{e.Username} [BOT]" : e.Username, Type = DiscordEntityType.User, ParentId = guild.Id });
        }

        private Dictionary<ulong, DiscordRole> GetRoles(SocketGuild guild) {
            return guild.Roles.ToDictionary(e => e.Id, e => new DiscordRole { Id = e.Id, Name = e.Name, Type = DiscordEntityType.Role, ParentId = guild.Id });
        }

        private Dictionary<ulong, DiscordChannel> GetChannels(SocketGuild guild) {
            return guild.Channels.ToDictionary(e => e.Id, e => new DiscordChannel { Id = e.Id, Name = e.Name, ParentId = guild.Id, ChannelType = MapChannelType(e.GetChannelType()) });
        }

        private DiscordChannelType? MapChannelType(ChannelType? channelType) {
            switch (channelType) {
                case ChannelType.Text: 
                    return DiscordChannelType.Text;
                case ChannelType.Voice: 
                    return DiscordChannelType.Voice;
                case ChannelType.Category: 
                    return DiscordChannelType.Category;
                case ChannelType.Stage: 
                    return DiscordChannelType.Stage;
                case ChannelType.Forum: 
                    return DiscordChannelType.Forum;
                case ChannelType.PrivateThread:
                case ChannelType.PublicThread:
                case ChannelType.NewsThread:
                    return DiscordChannelType.Thread;
            }

            return null;
        }
        #endregion
    }
}
