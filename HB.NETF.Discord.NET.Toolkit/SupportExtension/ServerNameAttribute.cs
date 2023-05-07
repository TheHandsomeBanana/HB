using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class ServerNameAttribute : Attribute {
        public string ServerName { get; set; }

        public ServerNameAttribute(string serverName) {
            ServerName = serverName;
        }
    }
}
