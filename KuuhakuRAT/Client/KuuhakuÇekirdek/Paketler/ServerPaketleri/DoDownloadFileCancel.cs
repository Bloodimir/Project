using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
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