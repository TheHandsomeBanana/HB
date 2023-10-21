using HB.NETF.Discord.NET.Toolkit.EntityService.Handler;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.NETF.Services.Data.Handler.Options;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Manipulator;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Obsolete.Cached.Handler {
    [Obsolete]
    public interface ICachedDiscordEntityServiceHandler : IStreamManipulator, IDisposable {
        void Init(params TokenModel[] tokens);
        Task Refresh();
        DiscordServer[] GetServers();
        DiscordUser[] GetUsers(ulong serverId);
        DiscordRole[] GetRoles(ulong serverId);
        DiscordChannel[] GetChannels(ulong serverId);
        DiscordEntity GetEntity(ulong id);
    }
}
