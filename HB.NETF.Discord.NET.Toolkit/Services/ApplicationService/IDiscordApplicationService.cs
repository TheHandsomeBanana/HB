using HB.NETF.Discord.NET.Toolkit.Models.Application;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.ApplicationService {
    public interface IDiscordApplicationService {
        Task<DiscordApplication[]> GetApplicationsAsync(string token);
    }
}
