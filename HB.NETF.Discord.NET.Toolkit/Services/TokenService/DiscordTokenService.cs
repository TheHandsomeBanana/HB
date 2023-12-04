using HB.NETF.Services.Security.Cryptography.Interfaces;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using HB.NETF.Services.Security.DataProtection;
using HB.NETF.Services.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.NETF.Discord.NET.Toolkit.Services.TokenService {
    public class DiscordTokenService : IDiscordTokenService {

        private readonly ISerializerService serializerService;
        private readonly IAesCryptoService aesCryptoService;
        private readonly IDataProtectionService dataProtectionService;

        public DiscordTokenService(IAesCryptoService aesCryptoService, IDataProtectionService dataProtectionService, ISerializerService serializerService) {
            this.serializerService = serializerService;
            this.aesCryptoService = aesCryptoService;
            this.dataProtectionService = dataProtectionService;
        }

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

        public string[] EncryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null)
            => tokens.Select(e => EncryptToken(e, encryptionMode, key)).ToArray();

        public string[] DecryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null)
            => tokens.Select(e => DecryptToken(e, encryptionMode, key)).ToArray();
    }
}
