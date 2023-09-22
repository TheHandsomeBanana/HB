using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Handler.Manipulator;
using HB.NETF.Services.Data.Handler.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Cached {
    public interface ICachedDiscordEntityService : IStreamManipulator, IDisposable {
        Task Refresh();
        Task<DiscordServerModel[]> GetServers();
        Task<DiscordUserModel[]> GetUsers(ulong serverId);
        Task<DiscordRoleModel[]> GetRoles(ulong serverId);
        Task<DiscordChannelModel[]> GetChannels(ulong serverId);
        Task<DiscordEntityModel> GetEntity(ulong id);
    }
}
