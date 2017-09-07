using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class PencereYardımcısı
    {
        public static string GetWindowTitle(string title, Client c)
        {
            return string.Format("{0} - {1}@{2} [{3}:{4}]", title, c.Value.KullanıcıAdi, c.Value.PcAdi, c.EndPoint.Address.ToString(), c.EndPoint.Port.ToString());
        }

        public static string GetWindowTitle(string title, int count)
        {
            return string.Format("{0} [Seçili: {1}]", title, count);
        }
    }
}
