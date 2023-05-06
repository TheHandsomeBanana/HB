using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ServerNamesAttribute : Attribute {
        public string[] ServerNames { get; set; }

        public ServerNamesAttribute(params string[] serverNames) {
            ServerNames = serverNames;
        }
    }
}
