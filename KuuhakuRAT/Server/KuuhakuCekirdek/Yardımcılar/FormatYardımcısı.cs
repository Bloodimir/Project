using System.IO;
using System.Text.RegularExpressions;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class FormatYardımcısı
    {
        public static string FormatMacAddress(string macAddress)
        {
            return (macAddress.Length != 12)
                ? "00:00:00:00:00:00"
                : Regex.Replace(macAddress, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");
        }

        public static string DriveTypeName(DriveType type)
        {
            switch (type)
            {
                case DriveType.Fixed:
                    return "Yerel Disk";
                case DriveType.Network:
                    return "Ağ Sürücüsü";
                case DriveType.Removable:
                    return "Kaldırılabilir Sürücü";
                default:
                    return type.ToString();
            }
        }

        public static string GenerateMutex(int length = 18)
        {
            return "KHK_MUTEX_" + DosyaYardımcısı.GetRandomFilename(length);
        }

        public static bool IsValidVersionNumber(string input)
        {
            Match match = Regex.Match(input, @"^[0-9]+\.[0-9]+\.(\*|[0-9]+)\.(\*|[0-9]+)$", RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
