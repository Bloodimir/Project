using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.KayıtDefteri;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetChangeRegistryValueResponse : IPacket
    {
        public string KeyPath { get; set; }
        public RegValueData Value { get; set; }

        public bool IsError { get; set; }
        public string ErrorMsg { get; set; }

        public GetChangeRegistryValueResponse() { }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
