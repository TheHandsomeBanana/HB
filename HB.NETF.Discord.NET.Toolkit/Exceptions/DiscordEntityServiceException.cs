using HB.NETF.Common.Exceptions;
using System;

namespace HB.NETF.Discord.NET.Toolkit.Exceptions {
    public class DiscordEntityServiceException : FrontException {
        public DiscordEntityServiceException(string message) : base(message) {
        }

        public DiscordEntityServiceException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
