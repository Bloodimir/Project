using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ClientPaketleri;

namespace xServer.KuuhakuCekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleLoadRegistryKey(GetRegistryKeysResponse packet, Client client)
        {
            try
            {
                if (packet.Matches != null && packet.Matches.Length > 0)
                {
                    if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                        !client.Value.FrmRe.Disposing)
                    {
                        client.Value.FrmRe.AddKeysToTree(packet.RootKey, packet.Matches);
                    }
                }
            }
            catch
            {
            }
        }

        #region Registry Key Edit

        public static void HandleCreateRegistryKey(GetCreateRegistryKeyResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.AddKeyToTree(packet.ParentPath, packet.Match);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        public static void HandleDeleteRegistryKey(GetDeleteRegistryKeyResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.RemoveKeyFromTree(packet.ParentPath, packet.KeyName);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        public static void HandleRenameRegistryKey(GetRenameRegistryKeyResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.RenameKeyFromTree(packet.ParentPath, packet.OldKeyName, packet.NewKeyName);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Registry Value Edit

        public static void HandleCreateRegistryValue(GetCreateRegistryValueResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.AddValueToList(packet.KeyPath, packet.Value);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        public static void HandleDeleteRegistryValue(GetDeleteRegistryValueResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.DeleteValueFromList(packet.KeyPath, packet.ValueName);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        public static void HandleRenameRegistryValue(GetRenameRegistryValueResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.RenameValueFromList(packet.KeyPath, packet.OldValueName, packet.NewValueName);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        public static void HandleChangeRegistryValue(GetChangeRegistryValueResponse packet, Client client)
        {
            try
            {
                if (client != null && client.Value.FrmRe != null && !client.Value.FrmRe.IsDisposed ||
                    !client.Value.FrmRe.Disposing)
                {
                    if (!packet.IsError)
                    {
                        client.Value.FrmRe.ChangeValueFromList(packet.KeyPath, packet.Value);
                    }
                    else
                    {
                        client.Value.FrmRe.ShowErrorMessage(packet.ErrorMsg);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}