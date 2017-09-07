using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler
{
    public interface IPacket
    {
        void Execute(Client client);
    }
}