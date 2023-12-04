using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using System.Collections.Generic;

namespace HB.NETF.Discord.NET.Toolkit.Services.TokenService {
    public interface IDiscordTokenService {
        string EncryptToken(string token, EncryptionMode encryptionMode, IKey key = null);
        string DecryptToken(string token, EncryptionMode encryptionMode, IKey key = null);
        string[] EncryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null);
        string[] DecryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null);
    }
}
