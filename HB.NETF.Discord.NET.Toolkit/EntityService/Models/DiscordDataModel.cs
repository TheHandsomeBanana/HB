using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Models {
    public class DiscordDataModel {
        public DiscordBotModel CreatedBy { get; set; }
        public DiscordServerModel[] Servers { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DiscordServerModel GetServer(ulong guildId) => Servers.FirstOrDefault(e => e.Id == guildId);
        public DiscordEntityModel GetEntity(ulong entityId) => GetServer(entityId)
                ?? Servers?.SelectMany(e => e.Roles).FirstOrDefault(e => e.Id == entityId) as DiscordEntityModel
                ?? Servers?.SelectMany(e => e.Channels).FirstOrDefault(e => e.Id == entityId) as DiscordEntityModel
                ?? Servers?.SelectMany(e => e.Users).FirstOrDefault(e => e.Id == entityId);
    }
}
