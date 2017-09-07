using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoClientReconnect : IPacket
    {
        public DoClientReconnect()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}