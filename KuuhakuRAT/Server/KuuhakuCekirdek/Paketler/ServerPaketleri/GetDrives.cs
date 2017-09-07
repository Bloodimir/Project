using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetDrives : IPacket
    {
        public GetDrives()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}