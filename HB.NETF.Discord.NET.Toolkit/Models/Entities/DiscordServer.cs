using System.Collections.Generic;

namespace HB.NETF.Discord.NET.Toolkit.Models.Entities {
    public class DiscordServer : DiscordEntity {
        public override DiscordEntityType Type => DiscordEntityType.Server;
        public Dictionary<ulong, DiscordUser> UserCollection { get; set; } = new Dictionary<ulong, DiscordUser>();
        public Dictionary<ulong, DiscordRole> RoleCollection { get; set; } = new Dictionary<ulong, DiscordRole>();
        public Dictionary<ulong, DiscordChannel> ChannelCollection { get; set; } = new Dictionary<ulong, DiscordChannel>();
    }
}
