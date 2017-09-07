using System;
using System.Threading;
using System.Windows.Forms;

namespace Kütüphane_Takip_Programı
{
    public partial class Yardım : Form
    {
        public Yardım()
        {
            InitializeComponent();

        }
        private static bool çıkış = true;
        public void yardım_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(Wait30Sec) { IsBackground = true };
            t.Start();
        }

        private void Yardım_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && çıkış)
                System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public void Wait30Sec()
        {
            for (int i = 29; i >= 0; i--)
            {
                Thread.Sleep(1000);
                if (!chkOkuma.Checked)
                try
                {
                    this.Invoke((MethodInvoker) delegate { btnKabulEt.Text = "Kabul Et (" + i + ")"; });
                }
                catch
                {
                }}
            }
        


        private void btnKabulEt_Click(object sender, EventArgs e)
        {
            var frm1 = new Form1();
            if (chkOkuma.Checked)
            çıkış = false;
            this.Close();
            frm1.Show();
            if (!chkOkuma.Checked)
                System.Diagnostics.Process.GetCurrentProcess().Kill();


        }

        private void chkOkuma_CheckedChanged(object sender, EventArgs e)
        {
            Thread t = new Thread(Wait30Sec) { IsBackground = true };
            try
            {
                this.Invoke((MethodInvoker) delegate { btnKabulEt.Text = "Kabul Et "; });
            }
            catch
            {
                
            }
            t.Abort();
            {

            }
            btnKabulEt.Enabled = true;
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}