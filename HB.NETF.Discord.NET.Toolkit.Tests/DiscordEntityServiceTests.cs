using HB.NETF.Common.Tests;
using HB.NETF.Discord.NET.Toolkit.EntityService;
using HB.NETF.Discord.NET.Toolkit.EntityService.Cached;
using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Tests {
    [TestClass]
    public class DiscordEntityServiceTests : TestBase {

        DiscordEntityService entityService;
        [TestMethod]
        public async Task DiscordEntityServiceTest() {
            TokenModel token = new TokenModel("Testbot", "");
            entityService = new DiscordEntityService(token);
            await entityService.ConnectAsync();
            await entityService.PullEntitiesAsync();
        }

        CachedDiscordEntityService cachedEntityService;
        [TestMethod]
        public async Task CachedDiscordEntityServiceTest() {
            TokenModel token = new TokenModel("Testbot", "");
            cachedEntityService = new CachedDiscordEntityService(token);
            await cachedEntityService.Refresh();
        }
    }
}
