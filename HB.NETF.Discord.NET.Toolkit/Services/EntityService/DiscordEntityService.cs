using Discord;
using Discord.WebSocket;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.Exceptions;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public class DiscordEntityService : IDiscordEntityService {

        private readonly DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
        });

        private readonly IAsyncStreamHandler streamHandler;
        private readonly NETF.Services.Logging.ILogger<DiscordEntityService> logger;
        private string token;
        
        public DiscordServerCollection ServerCollection { get; private set; } = new DiscordServerCollection();
        public DiscordEntityService() {
            this.logger = DIContainer.GetService<NETF.Services.Logging.Factory.ILoggerFactory>().GetOrCreateLogger<DiscordEntityService>();
            this.streamHandler = DIContainer.GetService<IAsyncStreamHandler>();
        }

        public void Init(string token) {
            this.token = token;
        }

        public async Task LoadEntities() {
            client.Ready += Client_Ready;
            client.Log += Client_Log;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
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

        private Task Client_Log(LogMessage message) {
            logger.Log(message.Message, MapSeverity(message.Severity));
            return Task.CompletedTask;
        }

        private async Task Client_Ready() {
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

            await client.StopAsync();
            await client.LogoutAsync();
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

        private HB.NETF.Services.Logging.LogSeverity MapSeverity(LogSeverity severity) {
            switch (severity) {
                case LogSeverity.Debug:
                    return NETF.Services.Logging.LogSeverity.Debug;
                case LogSeverity.Info:
                    return NETF.Services.Logging.LogSeverity.Information;
                case LogSeverity.Warning:
                    return NETF.Services.Logging.LogSeverity.Warning;
                case LogSeverity.Error:
                    return NETF.Services.Logging.LogSeverity.Error;
                case LogSeverity.Critical:
                    return NETF.Services.Logging.LogSeverity.Critical;
                default:
                    return NETF.Services.Logging.LogSeverity.Trace;
            }
        }
        #endregion

    }
}
