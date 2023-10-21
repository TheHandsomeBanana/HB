using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Models.Application {
    public class DiscordApplicationBot {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("username")]
        public string Name { get; set; }
    }
}
