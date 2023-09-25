using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.EntityService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.NETF.Discord.NET.Toolkit.EntityService.Handler;
using HB.NETF.Common.Tests;

namespace HB.NETF.Discord.NET.Toolkit.Tests {
    [TestClass]
    public class DiscordEntityServiceHandlerTests : TestBase {
        [TestMethod]
        public async Task DiscordEntityServiceHandlerTest() {
            TokenModel token1 = new TokenModel("BananaBot", "OTQ4NjcyNzU0MjcyNTgzNzIx.GdHysS.sXifYJCIbe500_GxEMvrSHCHOd5iqFJtQZywyk");
            TokenModel token2 = new TokenModel("AlbionBot", "OTQ5MDUyNzg5NjIxOTM2MTc4.GpOHku.djKnXwGAzOMur78h1EUJQMjMgILJtFeozhMo8k");
            DiscordEntityServiceHandler entityService = new DiscordEntityServiceHandler();
            entityService.Init(token1, token2);
            await entityService.ConnectAsync();
            await entityService.PullEntitiesAsync();
        }

        [TestMethod]
        public void CachedDiscordEntitysServiceHandlerTest() {

        }
    }
}
