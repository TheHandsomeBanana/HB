using Newtonsoft.Json;

namespace HB.NETF.Discord.NET.Toolkit.Models.Application {
    public class DiscordApplication {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("bot")]
        public DiscordApplicationBot Bot { get; set; }
    }
}
