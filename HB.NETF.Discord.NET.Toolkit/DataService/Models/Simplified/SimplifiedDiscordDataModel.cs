using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified {
    public class SimplifiedDiscordDataModel {
        public SimplifiedDiscordServerModel[] Servers { get; set; }
        public SimplifiedDiscordServerModel GetServer(ulong guildId) => Servers.FirstOrDefault(e => e.Id == guildId);
    }
}
