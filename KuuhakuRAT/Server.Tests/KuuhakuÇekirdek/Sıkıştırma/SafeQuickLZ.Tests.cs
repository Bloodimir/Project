using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xServer.KuuhakuCekirdek.Sıkıştırma;

namespace xServer.Tests.KuuhakuÇekirdek.Sıkıştırma
{
    [TestClass]
    public class SafeQuickLzTests
    {
        [TestMethod, TestCategory("Sıkıştırma")]
        public void SmallDataCompressionTestLevel1()
        {
            var smallData = new byte[100];

            new Random().NextBytes(smallData);

            var smallDataCompressed = SafeQuickLZ.Compress(smallData, 1);
            Assert.AreNotEqual(smallData, smallDataCompressed, "Original data is equal to the compressed data!");

            var smallDataDecompressed = SafeQuickLZ.Decompress(smallDataCompressed);
            Assert.AreNotEqual(smallDataCompressed, smallDataDecompressed,
                "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(smallData, smallDataDecompressed,
                "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Sıkıştırma")]
        public void SmallDataCompressionTestLevel3()
        {
            var smallData = new byte[100];

            new Random().NextBytes(smallData);

            var smallDataCompressed = SafeQuickLZ.Compress(smallData, 3);

            Assert.AreNotEqual(smallData, smallDataCompressed, "Original data is equal to the compressed data!");
            var smallDataDecompressed = SafeQuickLZ.Decompress(smallDataCompressed);

            Assert.AreNotEqual(smallDataCompressed, smallDataDecompressed,
                "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(smallData, smallDataDecompressed,
                "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Sıkıştırma")]
        public void BigDataCompressionTestLevel1()
        {
            var bigData = new byte[100000];

            new Random().NextBytes(bigData);

            var bigDataCompressed = SafeQuickLZ.Compress(bigData, 1);

            Assert.AreNotEqual(bigData, bigDataCompressed, "Original data is equal to the compressed data!");

            var bigDataDecompressed = SafeQuickLZ.Decompress(bigDataCompressed);

            Assert.AreNotEqual(bigDataCompressed, bigDataDecompressed,
                "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(bigData, bigDataDecompressed,
                "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Sıkıştırma")]
        public void BigDataCompressionTestLevel3()
        {
            var bigData = new byte[100000];

            new Random().NextBytes(bigData);
            var bigDataCompressed = SafeQuickLZ.Compress(bigData, 3);
            Assert.AreNotEqual(bigData, bigDataCompressed, "Original data is equal to the compressed data!");
            var bigDataDecompressed = SafeQuickLZ.Decompress(bigDataCompressed);
            Assert.AreNotEqual(bigDataCompressed, bigDataDecompressed,
                "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(bigData, bigDataDecompressed,
                "Original data does not match the decompressed data!");
        }
    }
}