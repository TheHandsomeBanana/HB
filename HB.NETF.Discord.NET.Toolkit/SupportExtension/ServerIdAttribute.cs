using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ServerIdAttribute : Attribute {
        public ulong ServerId { get; set; }

        public ServerIdAttribute(ulong serverId) {
            ServerId = serverId;
        }
    }
}
