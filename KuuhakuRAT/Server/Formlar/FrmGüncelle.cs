using System;
using System.IO;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Formlar
{
    public partial class FrmGüncelle : Form
    {
        private readonly int _selectedClients;

        public FrmGüncelle(int selected)
        {
            _selectedClients = selected;
            InitializeComponent();
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            Text = PencereYardımcısı.GetWindowTitle("Kurbanları Güncelle", _selectedClients);
            if (KuuhakuCekirdek.Veri.Update.İndirU)
                radioURL.Checked = true;
            txtPath.Text = File.Exists(KuuhakuCekirdek.Veri.Update.YüklemeDizini) ? KuuhakuCekirdek.Veri.Update.YüklemeDizini : string.Empty;
            txtURL.Text = KuuhakuCekirdek.Veri.Update.İndirmeURLsi;
  
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            KuuhakuCekirdek.Veri.Update.İndirU = radioURL.Checked;
            KuuhakuCekirdek.Veri.Update.YüklemeDizini = File.Exists(txtPath.Text) ? txtPath.Text : string.Empty;
            KuuhakuCekirdek.Veri.Update.İndirmeURLsi = txtURL.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Çalıştırılabilir (*.exe)|*.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = Path.Combine(ofd.InitialDirectory, ofd.FileName);
                }
            }
        }

        private void radioLocalFile_CheckedChanged(object sender, EventArgs e)
        {
            groupLocalFile.Enabled = radioLocalFile.Checked;
            groupURL.Enabled = !radioLocalFile.Checked;
        }

        private void radioURL_CheckedChanged(object sender, EventArgs e)
        {
            groupLocalFile.Enabled = !radioURL.Checked;
            groupURL.Enabled = radioURL.Checked;
        }
    }
}