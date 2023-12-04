using Newtonsoft.Json;

namespace HB.NETF.Discord.NET.Toolkit.Models.Application {
    public class DiscordApplicationBot {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("username")]
        public string Name { get; set; }
    }
}
