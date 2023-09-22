using HB.NETF.Discord.NET.Toolkit.EntityService.Handler;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
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

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Cached.Handler {
    public interface ICachedDiscordEntityServiceHandler : IStreamManipulator, IDisposable {
        void Init(params TokenModel[] tokens);
        Task Refresh();
        DiscordServerModel[] GetServers();
        DiscordUserModel[] GetUsers(ulong serverId);
        DiscordRoleModel[] GetRoles(ulong serverId);
        DiscordChannelModel[] GetChannels(ulong serverId);
        DiscordEntityModel GetEntity(ulong id);
    }
}
