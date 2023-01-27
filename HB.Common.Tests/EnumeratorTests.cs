using HB.Common.Enumerators;

namespace HB.Common.Tests {
    [TestClass]
    public class EnumeratorTests {
        [TestMethod]
        public void EnumeratorTest1() {
            foreach (int i in 0..10) {
                Console.WriteLine(i);
            }

            foreach (char c in 'a'..'x') {
                Console.WriteLine(c);
            }

            
        }
    }
}