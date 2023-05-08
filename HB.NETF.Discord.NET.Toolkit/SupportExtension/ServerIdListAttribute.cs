using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false)]
    public class ServerIdListAttribute : Attribute {
        public ulong[] ServerIds { get; set; }

        public ServerIdListAttribute(params ulong[] serverIds) {
            ServerIds = serverIds;
        }
    }
}
