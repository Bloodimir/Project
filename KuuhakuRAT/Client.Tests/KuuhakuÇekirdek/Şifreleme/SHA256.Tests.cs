using Microsoft.VisualStudio.TestTools.UnitTesting;
using xClient.KuuhakuÇekirdek.Kriptografi;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.Tests.KuuhakuÇekirdek.Şifreleme
{
    [TestClass]
    public class SHA256Tests
    {
        [TestMethod, TestCategory("Encryption")]
        public void ComputeHashTest()
        {
            var input = DosyaYardımcısı.RastgeleDosyaİsmi(100);
            var result = SHA256.ComputeHash(input);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, input);
        }
    }
}
