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

        [TestMethod]
        public async Task DiscordEntityServiceTest() {
            TokenModel token = new TokenModel("Testbot", "OTQ5MDUyNzg5NjIxOTM2MTc4.GpOHku.djKnXwGAzOMur78h1EUJQMjMgILJtFeozhMo8k");
            DiscordEntityService entityService = new DiscordEntityService(token);
            await entityService.ConnectAsync();
            await entityService.PullEntitiesAsync();
        }

        [TestMethod]
        public async Task CachedDiscordEntityServiceTest() {
            TokenModel token = new TokenModel("Testbot", "");
            CachedDiscordEntityService cachedEntityService = new CachedDiscordEntityService(token);
            await cachedEntityService.Refresh();
        }
    }
}
