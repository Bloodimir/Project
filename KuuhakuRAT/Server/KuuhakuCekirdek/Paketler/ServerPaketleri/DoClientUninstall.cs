using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoClientUninstall : IPacket
    {
        public DoClientUninstall()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}