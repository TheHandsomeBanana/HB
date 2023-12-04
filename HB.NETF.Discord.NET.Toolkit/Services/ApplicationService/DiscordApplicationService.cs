using HB.NETF.Discord.NET.Toolkit.Models.Application;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.ApplicationService {
    public class DiscordApplicationService : IDiscordApplicationService {
        public async Task<DiscordApplication[]> GetApplicationsAsync(string token) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage responseMessage = await client.GetAsync("https://discord.com/api/v9/applications?with_team_applications=true");

            string response = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DiscordApplication[]>(response);
        }
    }
}
