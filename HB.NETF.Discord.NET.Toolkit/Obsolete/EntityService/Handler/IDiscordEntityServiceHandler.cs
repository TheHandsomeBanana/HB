﻿using HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Obsolete.EntityService.Handler {
    public interface IDiscordEntityServiceHandler : IDisposable {
        void Init(params TokenModel[] tokens);
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