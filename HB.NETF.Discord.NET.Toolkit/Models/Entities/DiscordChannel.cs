using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Models.Entities {
    public class DiscordChannel : DiscordEntity {
        public override DiscordEntityType Type => DiscordEntityType.Channel;
        public DiscordChannelType? ChannelType { get; set; }
    }

    public enum DiscordChannelType {
        Text,
        Voice,
        Category,
        Stage,
        Forum,
        Thread,
        Guild,
        DM,
        Group,
        Private
    }
}
