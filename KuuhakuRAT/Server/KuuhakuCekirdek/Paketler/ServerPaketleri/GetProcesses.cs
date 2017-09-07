using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class GetProcesses : IPacket
    {
        public GetProcesses()
        {
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}