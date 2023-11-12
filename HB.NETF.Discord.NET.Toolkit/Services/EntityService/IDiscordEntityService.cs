﻿using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler.Manipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public delegate void ConnectionTimeout();
    public delegate void ConnectionReady();
    public interface IDiscordEntityService : IStreamManipulator, IDisposable, IAsyncDisposable {
        event ConnectionTimeout OnTimeout;
        int Timeout { get; set; }
        bool Ready { get; }
        Task Connect(string token);

        Task LoadEntities();
        Task Disconnect();


        Task SaveToFile(string fileName);
        Task<bool> ReadFromFile(string fileName);

        DiscordServer[] GetServers();
        DiscordUser[] GetUsers(ulong serverId);
        DiscordRole[] GetRoles(ulong serverId);
        DiscordChannel[] GetChannels(ulong serverId);
        DiscordChannel[] GetChannels(ulong serverId, DiscordChannelType? channelType);
        DiscordEntity GetEntity(ulong entityId);
    }
}
