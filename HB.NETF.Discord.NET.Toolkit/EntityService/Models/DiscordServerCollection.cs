using HB.NETF.Discord.NET.Toolkit.EntityService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Models {
    public class DiscordServerCollection : Dictionary<ulong, DiscordServerModel> {
        public DiscordEntityModel GetEntity(ulong entityId) {
            if (this.TryGetValue(entityId, out DiscordServerModel server))
                return server;

            return this.Values.SelectMany(e => e.Roles).FirstOrDefault(e => e.Id == entityId) as DiscordEntityModel
                ?? this.Values.SelectMany(e => e.Channels).FirstOrDefault(e => e.Id == entityId) as DiscordEntityModel
                ?? this.Values.SelectMany(e => e.Users).FirstOrDefault(e => e.Id == entityId);
        }

        public DiscordUserModel[] GetUsers(ulong serverId) {
            DiscordUserModel[] users = Array.Empty<DiscordUserModel>();
            if (this.TryGetValue(serverId, out DiscordServerModel server))
                users = server.Users;

            return users;
        }

        public DiscordRoleModel[] GetRoles(ulong serverId) {
            DiscordRoleModel[] users = Array.Empty<DiscordRoleModel>();
            if (this.TryGetValue(serverId, out DiscordServerModel server))
                users = server.Roles;

            return users;
        }

        public DiscordChannelModel[] GetChannels(ulong serverId) {
            DiscordChannelModel[] users = Array.Empty<DiscordChannelModel>();
            if (this.TryGetValue(serverId, out DiscordServerModel server))
                users = server.Channels;

            return users;
        }
    }
}
