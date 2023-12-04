using Discord;
using Discord.Rest;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging.Factory;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public class DiscordRestEntityService : DiscordBaseEntityService, IDiscordEntityService {

        public DiscordRestEntityService(ILoggerFactory loggerFactory, IAsyncStreamHandler streamHandler) : base(loggerFactory, streamHandler) {
            Client = new DiscordRestClient(new DiscordRestConfig() {
                DefaultRetryMode = RetryMode.AlwaysRetry
            });
        }

        public event ConnectionTimeout OnTimeout;

        public async Task Connect(string token) {
            Logger.LogInformation("Connecting");

            ((DiscordRestClient)Client).LoggedIn += Client_Ready;
            await ((DiscordRestClient)Client).LoginAsync(TokenType.Bot, token);

            Stopwatch sw = Stopwatch.StartNew();
            while (!Ready && sw.ElapsedMilliseconds <= Timeout) { } // Wait for connection
            sw.Stop();

            if (!Ready)
                OnTimeout?.Invoke();
        }

        public async Task Disconnect() {
            await ((DiscordRestClient)Client).LogoutAsync();
        }

        public async Task<DiscordServerCollection> LoadEntities() {
            if (!Ready) {
                Logger.LogError("Cannot load entities, connection timed out");
                return DiscordServerCollection.Empty;
            }

            IEnumerable<DiscordServer> servers = await Task.WhenAll((await ((DiscordRestClient)Client).GetGuildsAsync())
                .Select(async e => new DiscordServer() {
                    Id = e.Id,
                    Name = e.Name,
                    ParentId = null,
                    Type = DiscordEntityType.Server,
                    UserCollection = await GetUsers(e),
                    ChannelCollection = await GetChannels(e),
                    RoleCollection = await GetRoles(e)
                }));

            Logger.LogInformation($"{servers.Count()} servers data downloaded from Discord API.");
            return new DiscordServerCollection(servers);
        }

        #region Helper
        protected override async Task<Dictionary<ulong, DiscordChannel>> GetChannels(IGuild guild) {
            return (await ((RestGuild)guild).GetChannelsAsync())
                .ToDictionary(f => f.Id, f => new DiscordChannel() {
                    Id = f.Id,
                    Name = f.Name,
                    ParentId = guild.Id,
                    Type = DiscordEntityType.Channel,
                    ChannelType = MapChannelType(f.GetChannelType())
                });
        }

        protected override async Task<Dictionary<ulong, DiscordRole>> GetRoles(IGuild guild) {
            return guild.Roles.ToDictionary(f => f.Id, f => new DiscordRole() {
                Id = f.Id,
                Name = f.Name,
                ParentId = guild.Id,
                Type = DiscordEntityType.Role
            });
        }

        protected override async Task<Dictionary<ulong, DiscordUser>> GetUsers(IGuild guild) {
            return ((RestGuild)guild).GetUsersAsync().ToEnumerable()
                    .SelectMany(f => f.Select(l => new DiscordUser() {
                        Id = l.Id,
                        Name = l.Username,
                        ParentId = guild.Id,
                        Type = DiscordEntityType.User
                    }))
                    .ToDictionary(f => f.Id, f => f);
        }
        #endregion
    }
}
