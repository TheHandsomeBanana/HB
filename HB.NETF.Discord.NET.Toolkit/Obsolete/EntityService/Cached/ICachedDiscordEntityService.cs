using HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Handler.Manipulator;
using HB.NETF.Services.Data.Handler.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Cached {
    public interface ICachedDiscordEntityService : IStreamManipulator, IDisposable {
        Task Refresh();
        Task<DiscordServer[]> GetServers();
        Task<DiscordUser[]> GetUsers(ulong serverId);
        Task<DiscordRole[]> GetRoles(ulong serverId);
        Task<DiscordChannel[]> GetChannels(ulong serverId);
        Task<DiscordEntity> GetEntity(ulong id);
    }
}
