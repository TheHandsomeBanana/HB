using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;

namespace HB.NETF.Discord.NET.Toolkit.SupportExtension {
    /// <summary>
    /// Usage only on <see cref="IGuild"/> and derived data structures
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ServerIdAttribute : Attribute {
        public ulong ServerId { get; set; }
    }
}
