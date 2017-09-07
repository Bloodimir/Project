namespace xServer.KuuhakuCekirdek.Veri
{
    public class Sunucu
    {
        public string Hostname { get; set; }

        public ushort Port { get; set; }

        public override string ToString()
        {
            return Hostname + ":" + Port;
        }
    }
}
