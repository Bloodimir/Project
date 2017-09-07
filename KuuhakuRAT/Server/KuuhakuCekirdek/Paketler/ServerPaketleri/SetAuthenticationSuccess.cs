using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class SetAuthenticationSuccess : IPacket
    {
        public SetAuthenticationSuccess()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
