using Discord;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public abstract class DiscordBaseEntityService {
        [Dependency]

        public IAsyncStreamHandler StreamHandler;
        protected IDiscordClient Client { get; set; }
        protected ILogger<IDiscordEntityService> Logger { get; }

        public DiscordBaseEntityService(ILoggerFactory loggerFactory, IAsyncStreamHandler streamHandler) {
            this.Logger = loggerFactory.GetOrCreateLogger<IDiscordEntityService>();
            this.StreamHandler = streamHandler;
        }

        public bool Ready { get; private set; }
        public int Timeout { get; set; } = 10000;


        // Async stream handler currently not working if encrypted
        public async Task<DiscordServerCollection> ReadFromFile(string fileName) {
            return StreamHandler.WithOptions(optionBuilder).ReadFromFile<DiscordServerCollection>(fileName);
        }

        // Async stream handler currently not working if encrypted
        public async Task SaveToFile(string fileName, DiscordServerCollection serverCollection) {
            StreamHandler.WithOptions(optionBuilder).WriteToFile<DiscordServerCollection>(fileName, serverCollection);
        }

        private OptionBuilderFunc optionBuilder;

        public void ManipulateStream(OptionBuilderFunc optionBuilder) {
            this.optionBuilder = optionBuilder;
        }

        public void Dispose() {
            this.Client.Dispose();
        }

        public async ValueTask DisposeAsync() {
            await this.Client.DisposeAsync();
        }

        protected Task Client_Ready() {
            Ready = true;
            Logger.LogInformation("Connected");
            return Task.CompletedTask;
        }

        #region Helper 
        protected abstract Task<Dictionary<ulong, DiscordUser>> GetUsers(IGuild guild);
        protected abstract Task<Dictionary<ulong, DiscordRole>> GetRoles(IGuild guild);
        protected abstract Task<Dictionary<ulong, DiscordChannel>> GetChannels(IGuild guild);

        protected DiscordChannelType? MapChannelType(ChannelType? channelType) {
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