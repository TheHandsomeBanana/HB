using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.NETF.Discord.NET.Toolkit.Models.Entities;

namespace HB.NETF.Discord.NET.Toolkit.Models.Collections {
    public class DiscordServerCollection : Dictionary<ulong, DiscordServer> {
        public DiscordServerCollection() { }
        public DiscordServerCollection(IEnumerable<DiscordServer> servers) {
            foreach(DiscordServer server in servers) {
                if(!this.ContainsKey(server.Id))
                    this.Add(server.Id, server);
            }
        }

        public DiscordEntity GetEntity(ulong entityId) {
            if (this.TryGetValue(entityId, out DiscordServer server))
                return server;

            foreach(Dictionary<ulong, DiscordRole> roles in this.Values.Select(e => e.RoleCollection)) {
                if (roles.TryGetValue(entityId, out DiscordRole value))
                    return value;
            }

            foreach (Dictionary<ulong, DiscordChannel> channels in this.Values.Select(e => e.ChannelCollection)) {
                if (channels.TryGetValue(entityId, out DiscordChannel value))
                    return value;
            }

            foreach (Dictionary<ulong, DiscordUser> users in this.Values.Select(e => e.UserCollection)) {
                if (users.TryGetValue(entityId, out DiscordUser value))
                    return value;
            }

            return null;
        }

        public DiscordUser[] GetUsers(ulong serverId) {
            DiscordUser[] users = Array.Empty<DiscordUser>();
            if (this.TryGetValue(serverId, out DiscordServer server))
                users = server.UserCollection.Values.ToArray();

            return users;
        }

        public DiscordRole[] GetRoles(ulong serverId) {
            DiscordRole[] users = Array.Empty<DiscordRole>();
            if (this.TryGetValue(serverId, out DiscordServer server))
                users = server.RoleCollection.Values.ToArray();

            return users;
        }

        public DiscordChannel[] GetChannels(ulong serverId) {
            DiscordChannel[] users = Array.Empty<DiscordChannel>();
            if (this.TryGetValue(serverId, out DiscordServer server))
                users = server.ChannelCollection.Values.ToArray();

            return users;
        }

        public DiscordChannel[] GetChannels(ulong serverId, DiscordChannelType? channelType) {
            return channelType.HasValue 
                ? GetChannels(serverId).Where(e => e.ChannelType == channelType).ToArray()
                : GetChannels(serverId);
        }

        public DiscordServer[] GetServers() => this.Values.ToArray();
    }
}

