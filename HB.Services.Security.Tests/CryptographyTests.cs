using HB.Common.Serialization.Streams;
using HB.Services.Security.Cryptography;
using HB.Services.Security.Cryptography.Keys;
using System.Text;

namespace HB.Services.Security.Tests
{
    [TestClass]
    public class CryptographyTests {
        [TestMethod]
        public void TestAesCryptoServiceString() {
            AesKey key = KeyGenerator.GenerateAesKey();

            AesCryptoService aes = new AesCryptoService();
            string testdata = "This is a teststring";
            byte[] cipher = aes.Encrypt(Encoding.UTF8.GetBytes(testdata), key);
            byte[] decrdata = aes.Decrypt(cipher, key);

            Assert.AreEqual(testdata, Encoding.UTF8.GetString(decrdata));
        }

        [TestMethod]
        public void TestAesCryptoServiceImage() {
            AesCryptoService aes = new AesCryptoService();
            AesKey key = aes.GenerateKeys()[0];

            
            byte[] testdata;
            using (FileStream fs = new FileStream("D:\\Bilder\\ObitoDeathWP.jpg", FileMode.Open, FileAccess.Read)) {
                testdata = fs.Read();
            }

            byte[] imageCipher = aes.Encrypt(testdata, key);
            byte[] imageDecr = aes.Decrypt(imageCipher, key);

            Assert.AreEqual(testdata.Length, imageDecr.Length);
            for (int i = 0; i < testdata.Length; i++)
                Assert.AreEqual(testdata[i], imageDecr[i]);
        }

        [TestMethod]
        public void TestRsaCryptoServiceString() {
            RsaCryptoService rsa = new RsaCryptoService();
            RsaKey[] rsaKeys = rsa.GenerateKeys();

            string testdata = "This is a teststring";

            byte[] cipher = rsa.Encrypt(Encoding.UTF8.GetBytes(testdata), rsaKeys[0]);
            byte[] result = rsa.Decrypt(cipher, rsaKeys[1]);

            Assert.AreEqual(testdata, Encoding.UTF8.GetString(result));
        }
    }
}