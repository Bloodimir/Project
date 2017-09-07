using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetMonitors : IPacket
    {
        public GetMonitors()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}