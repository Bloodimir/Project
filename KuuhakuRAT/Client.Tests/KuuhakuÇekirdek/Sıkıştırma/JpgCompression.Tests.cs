using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xClient.KuuhakuÇekirdek.Sıkıştırma;

namespace xClient.Tests.KuuhakuÇekirdek.Sıkıştırma
{
    [TestClass]
    public class JpgCompressionTests
    {
        [TestMethod, TestCategory("Sıkıştırma")]
        public void CompressionTest()
        {
            var quality = Int64.MaxValue;
            var jpg = new JpgCompression(quality);
            var bitmap = new Bitmap(200, 200);

            var result = jpg.Compress(bitmap);

            Assert.IsNotNull(result);
            CollectionAssert.AllItemsAreNotNull(result);
        }
    }
}