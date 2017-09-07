using System;
using System.Management;
using System.Text.RegularExpressions;

namespace xClient.KuuhakuÇekirdek.Yardımcılar
{
    public static class PlatformYardımcısı
    {
        static PlatformYardımcısı()
        {
            Win32NT = Environment.OSVersion.Platform == PlatformID.Win32NT;
            XpYadaÜstü = Win32NT && Environment.OSVersion.Version.Major >= 5;
            VistaYadaÜstü = Win32NT && Environment.OSVersion.Version.Major >= 6;
            YediYadaÜstü = Win32NT && (Environment.OSVersion.Version >= new Version(6, 1));
            SekizYadaÜstü = Win32NT && (Environment.OSVersion.Version >= new Version(6, 2, 9200));
            W81YadaÜstü = Win32NT && (Environment.OSVersion.Version >= new Version(6, 3));
            OnYadaÜstü = Win32NT && (Environment.OSVersion.Version >= new Version(10, 0));
            MonodaÇalışıyor = Type.GetType("Mono.Runtime") != null;

            İsim = "Bilinmeyen OS";
            using (
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject os in searcher.Get())
                {
                    İsim = os["Caption"].ToString();
                    break;
                }
            }

            İsim = Regex.Replace(İsim, "^.*(?=Windows)", "").TrimEnd().TrimStart(); // Remove everything before first match "Windows" and trim end & start
            A64Bitmi = Environment.Is64BitOperatingSystem;
            Tamİsim = string.Format("{0} {1} Bit", İsim, A64Bitmi ? 64 : 32);
        }

        public static string Tamİsim { get; private set; } 
        public static string İsim { get; private set; }

        public static bool A64Bitmi { get; private set; }

        public static bool MonodaÇalışıyor { get; private set; }

        public static bool Win32NT { get; private set; }

        public static bool XpYadaÜstü { get; private set; }

        public static bool VistaYadaÜstü { get; private set; }

        public static bool YediYadaÜstü { get; private set; }

        public static bool SekizYadaÜstü { get; private set; }
        public static bool W81YadaÜstü { get; private set; }

        public static bool OnYadaÜstü { get; private set; }
    }
}
