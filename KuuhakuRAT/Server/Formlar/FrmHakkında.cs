using System;
using System.Windows.Forms;
using xServer.Properties;

namespace xServer.Formlar

{
    public partial class FrmHakkında : Form
    {
        public FrmHakkında()
        {
            InitializeComponent();

            lblVersion.Text = "v" + Application.ProductVersion;
            rtxtContent.Text = Resources.TOU;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rtxtContent_TextChanged(object sender, EventArgs e)
        {
        }
    }
}