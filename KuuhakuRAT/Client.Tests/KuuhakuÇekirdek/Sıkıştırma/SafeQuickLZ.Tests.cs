using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xClient.KuuhakuÇekirdek.Sıkıştırma;

namespace xClient.Tests.KuuhakuÇekirdek.Sıkıştırma
{
    [TestClass]
    public class SafeQuickLZTests
    {
        [TestMethod, TestCategory("Compression")]
        public void SmallDataCompressionTestLevel1()
        {
            byte[] smallData = new byte[100];
            new Random().NextBytes(smallData);
            byte[] smallDataCompressed = SafeQuickLZ.Compress(smallData, 1);
            Assert.AreNotEqual(smallData, smallDataCompressed, "Original data is equal to the compressed data!");
            byte[] smallDataDecompressed = SafeQuickLZ.Decompress(smallDataCompressed);

            Assert.AreNotEqual(smallDataCompressed, smallDataDecompressed, "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(smallData, smallDataDecompressed, "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Compression")]
        public void SmallDataCompressionTestLevel3()
        {
            byte[] smallData = new byte[100];
            new Random().NextBytes(smallData);

            byte[] smallDataCompressed = SafeQuickLZ.Compress(smallData, 3);

            Assert.AreNotEqual(smallData, smallDataCompressed, "Original data is equal to the compressed data!");
            byte[] smallDataDecompressed = SafeQuickLZ.Decompress(smallDataCompressed);
            Assert.AreNotEqual(smallDataCompressed, smallDataDecompressed, "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(smallData, smallDataDecompressed, "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Compression")]
        public void BigDataCompressionTestLevel1()
        {
            byte[] bigData = new byte[100000];

            new Random().NextBytes(bigData);
            byte[] bigDataCompressed = SafeQuickLZ.Compress(bigData, 1);

            Assert.AreNotEqual(bigData, bigDataCompressed, "Original data is equal to the compressed data!");

            byte[] bigDataDecompressed = SafeQuickLZ.Decompress(bigDataCompressed);
            Assert.AreNotEqual(bigDataCompressed, bigDataDecompressed, "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(bigData, bigDataDecompressed, "Original data does not match the decompressed data!");
        }

        [TestMethod, TestCategory("Compression")]
        public void BigDataCompressionTestLevel3()
        {
            byte[] bigData = new byte[100000];

            new Random().NextBytes(bigData);

            byte[] bigDataCompressed = SafeQuickLZ.Compress(bigData, 3);

            Assert.AreNotEqual(bigData, bigDataCompressed, "Original data is equal to the compressed data!");

            byte[] bigDataDecompressed = SafeQuickLZ.Decompress(bigDataCompressed);

            Assert.AreNotEqual(bigDataCompressed, bigDataDecompressed, "Compressed data is equal to the decompressed data!");
            CollectionAssert.AreEqual(bigData, bigDataDecompressed, "Original data does not match the decompressed data!");
        }
    }
}