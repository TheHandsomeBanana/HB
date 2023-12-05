using HB.NETF.Services.Logging;
using HB.NETF.Services.Logging.Factory;
using HB.NETF.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unity;

namespace HB.NETF.Common.Tests {
    [TestClass]
    public abstract class TestBase {

        protected IUnityContainer UnityContainer { get; }

        public TestBase() {
            UnityBase.Boot(new HB.NETF.Code.Analysis.UnitySetup(),
                new HB.NETF.Common.UnitySetup(),
                new HB.NETF.Discord.NET.Toolkit.UnitySetup(),
                new HB.NETF.Services.Data.UnitySetup(),
                new HB.NETF.Services.Logging.UnitySetup(),
                new HB.NETF.Services.Data.UnitySetup(),
                new HB.NETF.Services.Security.UnitySetup());

            UnityContainer = UnityBase.CreateChildContainer("TestContainer");

            ILoggerFactory factory = UnityContainer.Resolve<ILoggerFactory>();
            factory.InvokeLoggingBuilder(b => b.AddTarget(Output));

            UnityContainer.BuildUp(this);
        }

        public static void Output(LogStatement log) {
            Console.WriteLine(log.ToString());
        }
    }
}
