using HB.NETF.Discord.NET.Toolkit.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HB.NETF.Discord.NET.Toolkit.Models.Collections {
    public class DiscordServerCollection : Dictionary<ulong, DiscordServer> {
        public static DiscordServerCollection Empty => new DiscordServerCollection();

        public DiscordServerCollection() { }
        public DiscordServerCollection(IEnumerable<DiscordServer> servers) {
            foreach (DiscordServer server in servers) {
                if (!this.ContainsKey(server.Id))
                    this.Add(server.Id, server);
            }
        }

        public DiscordEntity GetEntity(ulong entityId) {
            if (this.TryGetValue(entityId, out DiscordServer server))
                return server;

            foreach (Dictionary<ulong, DiscordRole> roles in this.Values.Select(e => e.RoleCollection)) {
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
            DiscordRole[] roles = Array.Empty<DiscordRole>();
            if (this.TryGetValue(serverId, out DiscordServer server))
                roles = server.RoleCollection.Values.ToArray();

            return roles;
        }

        public DiscordChannel[] GetChannels(ulong serverId) {
            DiscordChannel[] channels = Array.Empty<DiscordChannel>();
            if (this.TryGetValue(serverId, out DiscordServer server))
                channels = server.ChannelCollection.Values.ToArray();

            return channels;
        }

        public DiscordChannel[] GetChannels(ulong serverId, DiscordChannelType? channelType) {
            DiscordChannel[] channels = GetChannels(serverId);
            if (channelType.HasValue && channels.Any(e => e.ChannelType == channelType))
                channels = channels.Where(e => e.ChannelType == channelType).ToArray();

            return channels;
        }

        public DiscordServer[] GetServers() => this.Values.ToArray();
        public DiscordRole[] GetRoles() => this.Values.SelectMany(e => e.RoleCollection.Values).ToArray();
        public DiscordUser[] GetUsers() => this.Values.SelectMany(e => e.UserCollection.Values).ToArray();
        public DiscordChannel[] GetChannels() => this.Values.SelectMany(e => e.ChannelCollection.Values).ToArray();
    }
}

