﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Models.Entities {
    public class DiscordUser : DiscordEntity {
        public override DiscordEntityType Type => DiscordEntityType.User;
    }
}