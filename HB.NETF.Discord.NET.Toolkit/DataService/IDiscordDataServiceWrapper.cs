using HB.NETF.Discord.NET.Toolkit.DataService.Models;
using HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService {
    public interface IDiscordDataServiceWrapper : IDisposable {
        void BuildUp(params TokenModel[] tokens);
        Task DownloadDataAsync();

        DiscordItemModel[] GetServers();
        DiscordItemModel[] GetUsers(ulong serverId);
        DiscordItemModel[] GetRoles(ulong serverId);
        DiscordItemModel[] GetChannels(ulong serverId);
    }
}
