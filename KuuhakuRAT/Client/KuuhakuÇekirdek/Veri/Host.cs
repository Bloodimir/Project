namespace xClient.KuuhakuÇekirdek.Veri
{
    public class Host
    {
        public string HostAdı { get; set; }
        public ushort Port { get; set; }

        public override string ToString()
        {
            return HostAdı + ":" + Port;
        }
    }
}