namespace HB.NETF.Discord.NET.Toolkit.Models.Entities {
    public class DiscordEntity {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ulong? ParentId { get; set; }
        public virtual DiscordEntityType Type { get; set; }
    }

    public enum DiscordEntityType {
        Server,
        User,
        Role,
        Channel
    }
}
