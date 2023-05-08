using HB.NETF.Discord.NET.Toolkit.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.DataService {
    public interface IExtendedDiscordDataService : IDiscordDataService {
        void BuildUp(params TokenModel[] tokens);
    }
}
