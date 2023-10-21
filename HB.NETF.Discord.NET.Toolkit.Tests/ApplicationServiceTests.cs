using HB.NETF.Discord.NET.Toolkit.Models.Application;
using HB.NETF.Discord.NET.Toolkit.Services.ApplicationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Tests {
    [TestClass]
    public class ApplicationServiceTests {
        [TestMethod]
        public async Task GetAllApplicationsWithToken() {
            IDiscordApplicationService applicationService = new DiscordApplicationService();
            DiscordApplication[] applications = await applicationService.GetApplicationsAsync("");
            
            
        }
    }
}
