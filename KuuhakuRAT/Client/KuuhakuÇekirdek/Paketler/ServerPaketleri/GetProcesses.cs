using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
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