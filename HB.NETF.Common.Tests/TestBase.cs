using HB.NETF.Services.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
