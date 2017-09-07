using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class SetStatusFileManager : IPacket
    {
        public string Message { get; set; }

        public bool SetLastDirectorySeen { get; set; }

        public SetStatusFileManager()
        {
        }

        public SetStatusFileManager(string message, bool setLastDirectorySeen)
        {
            Message = message;
            SetLastDirectorySeen = setLastDirectorySeen;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}