using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities {
    public class DiscordServerModel : DiscordEntityModel {
        [JsonIgnore]
        new public DiscordItemModelType ItemModelType => DiscordItemModelType.Server;
        public DiscordUserModel[] Users { get; set; }
        public DiscordRoleModel[] Roles { get; set; }
        public DiscordChannelModel[] Channels { get; set; }
    }
}
