using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class ServerNameListAttribute : Attribute {
        public string[] ServerNames { get; set; }

        public ServerNameListAttribute(params string[] serverNames) {
            ServerNames = serverNames;
        }
    }
}
