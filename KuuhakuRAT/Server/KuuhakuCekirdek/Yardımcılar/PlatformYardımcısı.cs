using System;

namespace xServer.KuuhakuCekirdek.Yardımcılar
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
        }

        public static int Mimari { get { return IntPtr.Size * 8; } }

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
