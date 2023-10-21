using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Obsolete {
    [Obsolete]
    public interface IDiscordEntityService : IDisposable {        
        Task ConnectAsync();
        Task PullEntitiesAsync();
        Task DisconnectAsync();
        DiscordServer[] GetServers();
        DiscordUser[] GetUsers(ulong serverId);
        DiscordRole[] GetRoles(ulong serverId);
        DiscordChannel[] GetChannels(ulong serverId);
        DiscordEntity GetEntity(ulong entityId);
    }
}
