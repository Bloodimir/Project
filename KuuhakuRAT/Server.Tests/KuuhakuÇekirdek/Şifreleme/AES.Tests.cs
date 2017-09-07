using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xServer.KuuhakuCekirdek.Kriptografi;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Tests.KuuhakuÇekirdek.Şifreleme
{
    [TestClass]
    public class AesTests
    {
        [TestMethod, TestCategory("Şifreleme")]
        public void EncryptAndDecryptStringTest()
        {
            var input = DosyaYardımcısı.GetRandomFilename(100);
            var password = DosyaYardımcısı.GetRandomFilename(50);

            AES.SetDefaultKey(password);

            var encrypted = AES.Encrypt(input);

            Assert.IsNotNull(encrypted);
            Assert.AreNotEqual(encrypted, input);

            var decrypted = AES.Decrypt(encrypted);

            Assert.AreEqual(input, decrypted);
        }

        [TestMethod, TestCategory("Şifreleme")]
        public void EncryptAndDecryptByteArrayTest()
        {
            var input = DosyaYardımcısı.GetRandomFilename(100);
            var inputByte = Encoding.UTF8.GetBytes(input);
            var password = DosyaYardımcısı.GetRandomFilename(50);

            AES.SetDefaultKey(password);

            var encryptedByte = AES.Encrypt(inputByte);

            Assert.IsNotNull(encryptedByte);
            CollectionAssert.AllItemsAreNotNull(encryptedByte);
            CollectionAssert.AreNotEqual(encryptedByte, inputByte);

            var decryptedByte = AES.Decrypt(encryptedByte);

            CollectionAssert.AreEqual(inputByte, decryptedByte);
        }
    }
}