using System;
using System.IO;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
namespace xServer.Formlar
{
    public partial class FrmYükleÇalıştır : Form
    {
        private readonly int _selectedClients;

        public FrmYükleÇalıştır(int selected)
        {
            _selectedClients = selected;
            InitializeComponent();
        }

        private void FrmUploadAndExecute_Load(object sender, EventArgs e)
        {
            Text = PencereYardımcısı.GetWindowTitle("Yükle & Çalıştır", _selectedClients);
            chkRunHidden.Checked = YükleÇalıştır.RunHidden;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Çalıştırılabilir (*.exe)|*.exe|Batch (*.bat)|*.bat";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = ofd.FileName;
                }
            }
        }

        private void btnUploadAndExecute_Click(object sender, EventArgs e)
        {
            YükleÇalıştır.FilePath = File.Exists(txtPath.Text) ? txtPath.Text : string.Empty;
            YükleÇalıştır.RunHidden = chkRunHidden.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}