using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ServerIdsAttribute : Attribute {
        public ulong[] ServerIds { get; set; }

        public ServerIdsAttribute(params ulong[] serverIds) {
            ServerIds = serverIds;
        }
    }
}
