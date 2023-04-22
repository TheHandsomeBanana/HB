﻿using HB.NETF.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit {
    public static class DiscordEnvironment {
        public static readonly string BasePath = GlobalEnvironment.BasePath + "\\Discord.NET";
        public static readonly string LogPath = BasePath + "\\Logs";
        public static readonly string CachePath = BasePath + "\\Cache";

        static DiscordEnvironment() {
            Directory.CreateDirectory(BasePath);
            Directory.CreateDirectory(LogPath);
            Directory.CreateDirectory(CachePath);
        }
    }
}