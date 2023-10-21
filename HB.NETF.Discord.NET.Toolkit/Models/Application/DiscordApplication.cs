using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
