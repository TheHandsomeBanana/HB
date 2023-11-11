using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Manipulator;
using HB.NETF.Services.Security.Cryptography.Keys;
using HB.NETF.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.TokenService {
    public interface IDiscordTokenService {
        string EncryptToken(string token, EncryptionMode encryptionMode, IKey key = null);
        string DecryptToken(string token, EncryptionMode encryptionMode, IKey key = null);
        string[] EncryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null);
        string[] DecryptTokens(IEnumerable<string> tokens, EncryptionMode encryptionMode, IKey key = null);
    }
}
