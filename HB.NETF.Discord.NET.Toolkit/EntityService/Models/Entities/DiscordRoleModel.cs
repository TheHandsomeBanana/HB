using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities {
    public class DiscordRoleModel : DiscordEntityModel {
        [JsonIgnore]
        new public DiscordItemModelType ItemModelType => DiscordItemModelType.Role;
    }
}
