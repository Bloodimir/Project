using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class DoShellExecuteResponse : IPacket
    {
        public string Output { get; set; }

        public bool IsError { get; private set; }

        public DoShellExecuteResponse()
        {
        }

        public DoShellExecuteResponse(string output, bool isError = false)
        {
            this.Output = output;
            this.IsError = isError;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}