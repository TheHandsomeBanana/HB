using Discord;
using Discord.WebSocket;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging.Factory;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public class DiscordSocketEntityService : DiscordBaseEntityService, IDiscordEntityService {

        public event ConnectionTimeout OnTimeout;

        public DiscordSocketEntityService(ILoggerFactory loggerFactory, IAsyncStreamHandler streamHandler) : base(loggerFactory, streamHandler) {
            Client = new DiscordSocketClient(new DiscordSocketConfig() {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            });
        }

        public async Task Connect(string token) {
            ((DiscordSocketClient)Client).Ready += Client_Ready;
            await ((DiscordSocketClient)Client).LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
            Logger.LogInformation("Connecting");

            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < Timeout && !Ready) { } // Wait for connection to establish
            if (!Ready)
                OnTimeout.Invoke();

            sw.Stop();
        }

        public async Task<DiscordServerCollection> LoadEntities() {
            if (!Ready) {
                Logger.LogError("Cannot load entities, connection timed out");
                return DiscordServerCollection.Empty;
            }

            List<DiscordServer> servers = new List<DiscordServer>();
            foreach (var server in ((DiscordSocketClient)Client).Guilds) {
                servers.Add(new DiscordServer() {
                    Id = server.Id,
                    Name = server.Name,
                    UserCollection = await GetUsers(server),
                    RoleCollection = await GetRoles(server),
                    ChannelCollection = await GetChannels(server)
                });
            }

            Logger.LogInformation($"{servers.Count} servers data downloaded from Discord API.");
            return new DiscordServerCollection(servers);
        }

        public async Task Disconnect() {
            if (!Ready) {
                Logger.LogError("Cannot disconnect, connection timed out.");
                return;
            }

            await Client.StopAsync();
            await ((DiscordSocketClient)Client).LogoutAsync();
            Logger.LogInformation("Disconnected.");
        }

        #region Helper 
        protected override async Task<Dictionary<ulong, DiscordUser>> GetUsers(IGuild guild) {
            await guild.DownloadUsersAsync();

            return ((SocketGuild)guild).Users.ToDictionary(e => e.Id, e => new DiscordUser { Id = e.Id, Name = e.IsBot ? $"{e.Username} [BOT]" : e.Username, Type = DiscordEntityType.User, ParentId = guild.Id });
        }

        protected override async Task<Dictionary<ulong, DiscordRole>> GetRoles(IGuild guild) {
            return guild.Roles.ToDictionary(e => e.Id, e => new DiscordRole { Id = e.Id, Name = e.Name, Type = DiscordEntityType.Role, ParentId = guild.Id });
        }

        protected override async Task<Dictionary<ulong, DiscordChannel>> GetChannels(IGuild guild) {
            return ((SocketGuild)guild).Channels.ToDictionary(e => e.Id, e => new DiscordChannel { Id = e.Id, Name = e.Name, ParentId = guild.Id, ChannelType = MapChannelType(e.GetChannelType()) });
        }

        #endregion

    }
}
