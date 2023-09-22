using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Services.Data.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.TokenService {
    public class DiscordTokenService : IDiscordTokenService {

        private IStreamHandler streamHandler;
        public DiscordTokenService() {
            streamHandler = DIContainer.GetService<IStreamHandler>();
        }

        public TokenModel ReadToken(string filePath) {
            TokenModel token = streamHandler.WithOptions(optionBuilder).ReadFromFile<TokenModel>(filePath);
            return token;
        }

        public void WriteToken(string filePath, TokenModel token) {
            streamHandler.WithOptions(optionBuilder).WriteToFile(filePath, token);
        }

        private OptionBuilderFunc optionBuilder;
        public void ManipulateStream(OptionBuilderFunc optionBuilder) => this.optionBuilder = optionBuilder;
    }
}
