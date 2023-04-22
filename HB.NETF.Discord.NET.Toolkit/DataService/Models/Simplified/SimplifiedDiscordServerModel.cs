using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService.Models.Simplified {
    public class SimplifiedDiscordServerModel : DiscordItemModel {
        public DiscordItemModel[] Users { get; set; }
        public DiscordItemModel[] Roles { get; set; }
        public DiscordItemModel[] Channels { get; set; }
    }
}
