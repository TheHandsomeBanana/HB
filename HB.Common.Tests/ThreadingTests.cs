using HB.Common.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Common.Tests {
    [TestClass]
    public class ThreadingTests {
        [TestMethod]
        public async Task StaticLockTests() {
            List<Task> writeTasks = new List<Task>();
            writeTasks.Add(Task.Run(WriteLocked));
            writeTasks.Add(Task.Run(Write));
            await Task.WhenAll(writeTasks);
        }

        public void Write() {
            using (StreamWriter sw = new StreamWriter("test")) {
                sw.WriteLine("Test");
            }
        }

        public void WriteLocked() {
            Locker.RunLocked(Write);
        }
    }
}
