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

        public string EncryptToken(string token, EncryptionMode encryptionMode, IKey key = null) {
            switch (encryptionMode) {
                case EncryptionMode.AES:
                    return serializerService
                        .ToBase64(aesCryptoService
                        .Encrypt(serializerService.GetResultBytes(token), key));

                case EncryptionMode.RSA:
                    break;
                case EncryptionMode.WindowsDataProtectionAPI:
                    return serializerService
                        .ToBase64(dataProtectionService
                        .Protect(serializerService.GetResultBytes(token)));
            }

            throw new NotSupportedException($"{encryptionMode} not supported.");
        }

        public string DecryptToken(string token, EncryptionMode encryptionMode, IKey key = null) {
            switch (encryptionMode) {
                case EncryptionMode.AES:
                    return serializerService
                        .GetResultString(aesCryptoService
                        .Decrypt(serializerService.FromBase64(token), key));

                case EncryptionMode.RSA:
                    break;
                case EncryptionMode.WindowsDataProtectionAPI:
                    return serializerService
                        .GetResultString(dataProtectionService
                        .Unprotect(serializerService.FromBase64(token)));
            }

            throw new NotSupportedException($"{encryptionMode} not supported.");
        }

        public TokenModel EncryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null) {
            return new TokenModel(tokenModel.Bot, this.EncryptToken(tokenModel.Token, encryptionMode, key), tokenModel.CreatedOn);
        }

        public TokenModel DecryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null) {
            return new TokenModel(tokenModel.Bot, this.DecryptToken(tokenModel.Token, encryptionMode, key), tokenModel.CreatedOn);
        }

        public string[] EncryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null)
            => tokens.Select(e => EncryptToken(e, encryptionMode, key)).ToArray();

        public string[] DecryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null)
            => tokens.Select(e => DecryptToken(e, encryptionMode, key)).ToArray();

        public TokenModel[] EncryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null) {
            return tokens.Select(e => EncryptToken(e, encryptionMode, key)).ToArray();
        }

        public TokenModel[] DecryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null) {
            return tokens.Select(e => DecryptToken(e, encryptionMode, key)).ToArray();
        }

       
    }
}
