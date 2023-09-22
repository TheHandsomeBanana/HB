using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Manipulator;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.TokenService {
    public interface IDiscordTokenService : IStreamManipulator {
        TokenModel ReadToken(string filePath);
        void WriteToken(string filePath, TokenModel model);
    }
}
