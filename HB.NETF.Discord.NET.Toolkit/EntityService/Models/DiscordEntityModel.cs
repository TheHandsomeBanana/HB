using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Models {
    public class DiscordEntityModel {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public DiscordItemModelType ItemModelType { get; set; }
    }

    public enum DiscordItemModelType {
        Server,
        User,
        Role,
        Channel
    }
}
