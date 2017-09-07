using System;
using System.IO;
using System.Linq;
using System.Text;
using xServer.KuuhakuCekirdek.Kriptografi;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class DosyaYardımcısı
    {
        private const string Karakterler = "ABCÇDEFGĞHIİJKLMNOPQRSŞTUVWXYZabcçdefgğhiıjklmnopqrsştuvwxyz0123456789";
        private static readonly Random RNG = new Random(Environment.TickCount);
        private static readonly string[] _sizes = { "B", "KB", "MB", "GB" };
        private static readonly char[] _illegalChars = Path.GetInvalidPathChars().Union(Path.GetInvalidFileNameChars()).ToArray();

        public static bool CheckPathForIllegalChars(string path)
        {
            return path.Any(c => _illegalChars.Contains(c));
        }

        public static string GetRandomFilename(int length, string extension = "")
        {
            StringBuilder randomName = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                randomName.Append(Karakterler[RNG.Next(Karakterler.Length)]);

            return string.Concat(randomName.ToString(), extension);
        }

        public static int GetNewTransferId(int o = 0)
        {
            return RNG.Next(0, int.MaxValue) + o;
        }

        public static string GetDataSize(long size)
        {
            double len = size;
            int order = 0;
            while (len >= 1024 && order + 1 < _sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return string.Format("{0:0.##} {1}", len, _sizes[order]);
        }

        public static int GetFileIcon(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return 2;

            switch (extension.ToLower())
            {
                default:
                    return 2;
                case ".exe":
                    return 3;
                case ".txt":
                case ".log":
                case ".conf":
                case ".cfg":
                case ".asc":
                    return 4;
                case ".rar":
                case ".zip":
                case ".zipx":
                case ".tar":
                case ".tgz":
                case ".gz":
                case ".s7z":
                case ".7z":
                case ".bz2":
                case ".cab":
                case ".zz":
                case ".apk":
                    return 5;
                case ".doc":
                case ".docx":
                case ".odt":
                    return 6;
                case ".pdf":
                    return 7;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                case ".gif":
                case ".ico":
                    return 8;
                case ".mp4":
                case ".mov":
                case ".avi":
                case ".wmv":
                case ".mkv":
                case ".m4v":
                case ".flv":
                    return 9;
                case ".mp3":
                case ".wav":
                case ".pls":
                case ".m3u":
                case ".m4a":
                    return 10;
            }
        }
        public static void WriteLogFile(string filename, string appendText)
        {
            appendText = ReadLogFile(filename) + appendText;

            using (FileStream fStream = File.Open(filename, FileMode.Create, FileAccess.Write))
            {
                byte[] data = AES.Encrypt(Encoding.UTF8.GetBytes(appendText));
                fStream.Seek(0, SeekOrigin.Begin);
                fStream.Write(data, 0, data.Length);
            }
        }

        public static string ReadLogFile(string filename)
        {
            return File.Exists(filename) ? Encoding.UTF8.GetString(AES.Decrypt(File.ReadAllBytes(filename))) : string.Empty;
        }
    }
}
