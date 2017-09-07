using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetMonitorsResponse : IPacket
    {
        public int Number { get; set; }

        public GetMonitorsResponse()
        {
        }

        public GetMonitorsResponse(int number)
        {
            this.Number = number;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}