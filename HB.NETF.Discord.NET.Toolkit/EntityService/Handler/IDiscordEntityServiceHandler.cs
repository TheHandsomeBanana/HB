﻿using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Handler {
    public interface IDiscordEntityServiceHandler : IDisposable {
        void Init(params TokenModel[] tokens);
        Task ConnectAsync();
        Task PullEntitiesAsync();
        Task DisconnectAsync();
        DiscordServerModel[] GetServers();
        DiscordUserModel[] GetUsers(ulong serverId);
        DiscordRoleModel[] GetRoles(ulong serverId);
        DiscordChannelModel[] GetChannels(ulong serverId);
        DiscordEntityModel GetEntity(ulong entityId);
    }
}
