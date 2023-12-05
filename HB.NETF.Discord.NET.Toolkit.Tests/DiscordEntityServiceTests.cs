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

            IDiscordEntityService entityService = <IDiscordEntityService>();
            await entityService.Connect("");
            await entityService.LoadEntities();
        }
    }
}
