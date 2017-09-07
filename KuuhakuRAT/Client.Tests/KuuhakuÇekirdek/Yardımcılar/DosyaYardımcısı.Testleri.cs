using Microsoft.VisualStudio.TestTools.UnitTesting;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.Tests.KuuhakuÇekirdek.Yardımcılar
{
    [TestClass]
    public class FileHelperTests
    {
        [TestMethod, TestCategory("Yardımcı")]
        public void RandomFilenameTest()
        {
            var length = 100;
            var name = DosyaYardımcısı.RastgeleDosyaİsmi(length);
            Assert.IsNotNull(name);
            Assert.IsTrue(name.Length == length, "Dosya İsmi uzunluğu yanlış!");
        }

        [TestMethod, TestCategory("Yardımcı")]
        public void ValidateExecutableTest()
        {
            var bytes = new byte[] {77, 90};

            var result = DosyaYardımcısı.ExeValidmiKardeş(bytes);

            Assert.IsTrue(result, ".exe dosyası geçerliliği kontrol etme başarısız!");
        }

        [TestMethod, TestCategory("Yardımcı")]
        public void ValidateInvalidFileTest()
        {
            var bytes = new byte[] {22, 93};

            var result = DosyaYardımcısı.ExeValidmiKardeş(bytes);

            Assert.IsFalse(result, "Geçersiz dosya kontrolü başarılı!");
        }
    }
}