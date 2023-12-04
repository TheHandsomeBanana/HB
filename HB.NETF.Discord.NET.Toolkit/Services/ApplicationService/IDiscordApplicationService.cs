using HB.NETF.Discord.NET.Toolkit.Models.Application;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.ApplicationService {
    public interface IDiscordApplicationService {
        Task<DiscordApplication[]> GetApplicationsAsync(string token);
    }
}
