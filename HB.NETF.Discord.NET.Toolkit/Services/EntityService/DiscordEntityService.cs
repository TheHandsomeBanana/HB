using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public class DiscordEntityService : IDiscordEntityService {

        private readonly DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
        });

        private readonly IAsyncStreamHandler streamHandler;
        private readonly ILogger<DiscordEntityService> logger;
        
        public DiscordServerCollection ServerCollection { get; private set; } = new DiscordServerCollection();
        public DiscordEntityService(ILoggerFactory loggerFactory, IAsyncStreamHandler streamHandler) {
            this.logger = loggerFactory.GetOrCreateLogger<DiscordEntityService>();
            this.streamHandler = streamHandler;
        }

        public event ConnectionTimeout OnTimeout;
        public bool Ready { get; private set; }
        public int Timeout { get; set; } = 10000;

        public async Task Connect(string token) {
            client.Ready += Client_Ready;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            logger.LogInformation("Connecting.");

            Stopwatch sw = Stopwatch.StartNew();
            while(sw.ElapsedMilliseconds < Timeout && !Ready) { } // Wait for connection to establish
            if (!Ready)
                OnTimeout.Invoke();

            sw.Stop();
        }

        public async Task LoadEntities() {
            if (!Ready) {
                logger.LogError("Cannot load entities, connection timed out.");
                return;
            }

            logger.LogInformation("Connected.");
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

            logger.LogInformation($"{servers.Count} servers data downloaded from Discord API.");
            ServerCollection = new DiscordServerCollection(servers);
        }

        public async Task Disconnect() {
            if (!Ready) {
                logger.LogError("Cannot disconnect, connection timed out.");
                return;
            }

            await client.StopAsync();
            await client.LogoutAsync();
            logger.LogInformation("Disconnected.");
        }


        public async Task<bool> ReadFromFile(string fileName) {
            bool fileExists = File.Exists(fileName);
            if (fileExists)
                ServerCollection = await Task.Run(() => streamHandler.WithOptions(optionBuilder).ReadFromFile<DiscordServerCollection>(fileName));

            return fileExists;
        }
        public async Task SaveToFile(string fileName) {
            await Task.Run(() => streamHandler.WithOptions(optionBuilder).WriteToFile<DiscordServerCollection>(fileName, ServerCollection));
        }

        public DiscordEntity GetEntity(ulong entityId) => ServerCollection.GetEntity(entityId);
        public DiscordServer[] GetServers() => ServerCollection.GetServers();
        public DiscordUser[] GetUsers(ulong serverId) => ServerCollection.GetUsers(serverId);
        public DiscordRole[] GetRoles(ulong serverId) => ServerCollection.GetRoles(serverId);
        public DiscordChannel[] GetChannels(ulong serverId) => ServerCollection.GetChannels(serverId);
        public DiscordChannel[] GetChannels(ulong serverId, DiscordChannelType? channelType) => ServerCollection.GetChannels(serverId, channelType);

        private OptionBuilderFunc optionBuilder;

        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }

        public void Dispose() {
            this.client.Dispose();
        }

        public async ValueTask DisposeAsync() {
            await this.client.DisposeAsync();
        }

        private Task Client_Ready() {
            Ready = true;
            return Task.CompletedTask;
        }

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
