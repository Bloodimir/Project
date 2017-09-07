using System.Collections.Generic;

namespace xServer.KuuhakuCekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static Dictionary<int, string> CanceledDownloads = new Dictionary<int, string>();
        public static Dictionary<int, string> RenamedFiles = new Dictionary<int, string>();
        private const string DELIMITER = "$E$";
    }
}