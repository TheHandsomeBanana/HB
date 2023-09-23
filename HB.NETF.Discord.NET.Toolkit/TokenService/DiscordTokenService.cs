using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Security.Cryptography;
using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataProtection;
using HB.NETF.Services.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.TokenService {
    public class DiscordTokenService : IDiscordTokenService {

        private readonly IStreamHandler streamHandler;
        private readonly ISerializerService serializerService;
        private readonly IAesCryptoService aesCryptoService;
        private readonly IRsaCryptoService rsaCryptoService;
        private readonly IDataProtectionService dataProtectionService;

        public DiscordTokenService() {
            streamHandler = DIContainer.GetService<IStreamHandler>();
            aesCryptoService = DIContainer.GetService<IAesCryptoService>();
            rsaCryptoService = DIContainer.GetService<IRsaCryptoService>();
            dataProtectionService = DIContainer.GetService<IDataProtectionService>();
            serializerService = DIContainer.GetService<ISerializerService>();
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

        public TokenModel EncryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null) {
            switch(encryptionMode) {
                case EncryptionMode.AES:
                    string encrToken = serializerService
                        .ToBase64(aesCryptoService
                        .Encrypt(serializerService.GetResultBytes(tokenModel.Token), key));

                    return new TokenModel(tokenModel.Bot, encrToken, tokenModel.CreatedOn);
                case EncryptionMode.RSA:
                    break;
                case EncryptionMode.WindowsDataProtectionAPI:
                    break;
            }

            throw new NotSupportedException($"{encryptionMode} not supported.");
        }

        public TokenModel DecryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null) {
            switch (encryptionMode) {
                case EncryptionMode.AES:
                    string encrToken = serializerService
                        .GetResultString(aesCryptoService
                        .Decrypt(serializerService.FromBase64(tokenModel.Token), key));

                    return new TokenModel(tokenModel.Bot, encrToken, tokenModel.CreatedOn);
                case EncryptionMode.RSA:
                    break;
                case EncryptionMode.WindowsDataProtectionAPI:
                    break;
            }

            throw new NotSupportedException($"{encryptionMode} not supported.");
        }

        public TokenModel[] EncryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null) {
            return tokens.Select(e => EncryptToken(e, encryptionMode, key)).ToArray();
        }

        public TokenModel[] DecryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null) {
            return tokens.Select(e => DecryptToken(e, encryptionMode, key)).ToArray();
        }
    }
}
