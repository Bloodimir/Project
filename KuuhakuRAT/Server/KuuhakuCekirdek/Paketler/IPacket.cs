using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler
{
    public interface IPacket
    {
        void Execute(Client client);
    }
}