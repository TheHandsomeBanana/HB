using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using HB.NETF.Services.Data.Handler.Manipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public interface IDiscordEntityService : IStreamManipulator, IDisposable, IAsyncDisposable {
        void Init(params string[] tokens);
        Task LoadEntities();
        Task SaveToFile(string fileName);
        Task<bool> ReadFromFile(string fileName);

        DiscordServer[] GetServers();
        DiscordUser[] GetUsers(ulong serverId);
        DiscordRole[] GetRoles(ulong serverId);
        DiscordChannel[] GetChannels(ulong serverId);
        DiscordEntity GetEntity(ulong entityId);
    }
}
