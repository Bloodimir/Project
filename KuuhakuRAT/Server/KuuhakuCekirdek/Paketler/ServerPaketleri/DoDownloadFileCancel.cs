using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoDownloadFileCancel : IPacket
    {
        public int ID { get; set; }

        public DoDownloadFileCancel()
        {
        }

        public DoDownloadFileCancel(int id)
        {
            this.ID = id;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}