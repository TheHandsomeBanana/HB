using HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService {
    public interface IDiscordDataService : IDisposable {
        Task DownloadDataAsync();
        Task<DiscordItemModel[]> GetServers();
        Task<DiscordItemModel[]> GetUsers(ulong serverId);
        Task<DiscordItemModel[]> GetRoles(ulong serverId);
        Task<DiscordItemModel[]> GetChannels(ulong serverId);

    }
}
