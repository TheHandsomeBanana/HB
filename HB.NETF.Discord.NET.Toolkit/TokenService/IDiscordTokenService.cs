using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
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

namespace HB.NETF.Discord.NET.Toolkit.TokenService {
    public interface IDiscordTokenService : IStreamManipulator {
        TokenModel EncryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null);
        TokenModel DecryptToken(TokenModel tokenModel, EncryptionMode encryptionMode, IKey key = null);
        TokenModel[] EncryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null);
        TokenModel[] DecryptTokens(TokenModel[] tokens, EncryptionMode encryptionMode, IKey key = null);
    
        TokenModel ReadToken(string filePath);
        void WriteToken(string filePath, TokenModel model);
    }
}
