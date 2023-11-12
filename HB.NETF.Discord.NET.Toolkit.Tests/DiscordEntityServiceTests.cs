using HB.NETF.Common.DependencyInjection;
using HB.NETF.Common.Tests;
using HB.NETF.Discord.NET.Toolkit.Services.EntityService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Tests {
    [TestClass]
    public class DiscordEntityServiceTests : TestBase {

        [TestMethod]
        public async Task DiscordEntityServiceTest() {

            IDiscordEntityService entityService = DIContainer.GetService<IDiscordEntityService>();
            entityService.Init("OTQ4NjcyNzU0MjcyNTgzNzIx.Gdn4wJ.C_zwRc3XgJ0RqIMpu6Kndepdd0c1fMPG1ZaJps");

            await entityService.LoadEntities();
        }
    }
}
