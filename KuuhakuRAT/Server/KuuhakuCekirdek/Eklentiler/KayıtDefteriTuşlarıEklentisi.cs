using Microsoft.Win32;
using System;

namespace xServer.KuuhakuCekirdek.Eklentiler
{
    public static class KayıtDefteriTuşlarıEklentisi
    {
        public static string RegistryTypeToString(this RegistryValueKind valueKind, object valueData)
        {
            switch (valueKind)
            {
                case RegistryValueKind.Binary:
                    return ((byte[])valueData).Length > 0 ? BitConverter.ToString((byte[])valueData).Replace("-", " ").ToLower() : "(0-uzunlukta binary değeri)";
                case RegistryValueKind.MultiString:
                    return string.Join(" ", (string[])valueData);
                case RegistryValueKind.DWord:
                    return String.Format("0x{0} ({1})", ((uint)((int)valueData)).ToString("x8"), ((uint)((int)valueData)).ToString());
                case RegistryValueKind.QWord:
                    return String.Format("0x{0} ({1})", ((ulong)((long)valueData)).ToString("x8"), ((ulong)((long)valueData)).ToString());
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return valueData.ToString();
                case RegistryValueKind.Unknown:
                default:
                    return string.Empty;
            }
        }

        public static string RegistryTypeToString(this RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.Binary:
                    return "REG_BINARY";
                case RegistryValueKind.MultiString:
                    return "REG_MULTI_SZ";
                case RegistryValueKind.DWord:
                    return "REG_DWORD";
                case RegistryValueKind.QWord:
                    return "REG_QWORD";
                case RegistryValueKind.String:
                    return "REG_SZ";
                case RegistryValueKind.ExpandString:
                    return "REG_EXPAND_SZ";
                case RegistryValueKind.Unknown:
                    return "(Bilinmiyor)";
                default:
                    return "REG_NONE";
            }
        }
    }
}
