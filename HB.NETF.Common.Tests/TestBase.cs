using HB.NETF.Code.Analysis.Factory;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Common.DependencyInjection;
using HB.NETF.Discord.NET.Toolkit.Services.EntityService;
using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Services.Security.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Common.Tests {
    [TestClass]
    public abstract class TestBase {
        
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Initialize(TestContext context) {
            

        }

        public static void Output(LogStatement log) {
            Console.WriteLine(log.ToString());
        }
    }
}
