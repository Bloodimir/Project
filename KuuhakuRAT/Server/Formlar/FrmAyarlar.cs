using System;
using System.Globalization;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Kriptografi;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Ağ.UtilitylerAğ;
using xServer.KuuhakuCekirdek.UTIlityler;

namespace xServer.Formlar
{
    public partial class FrmAyarlar : Form
    {
        private readonly KuuhakuServer _listenServer;

        public FrmAyarlar(KuuhakuServer listenServer)
        {
            this._listenServer = listenServer;

            InitializeComponent();

            if (listenServer.Listening)
            {
                btnListen.Text = "Dinlemeyi Durdur";
                ncPort.Enabled = false;
                txtPassword.Enabled = false;
            }

            ShowPassword(false);
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            ncPort.Value = Ayarlar.ListenPort;
            chkAutoListen.Checked = Ayarlar.AutoListen;
            chkPopup.Checked = Ayarlar.ShowPopup;
            txtPassword.Text = Ayarlar.Password;
            chkUseUpnp.Checked = Ayarlar.UseUPnP;
            chkShowTooltip.Checked = Ayarlar.ShowToolTip;
            chkNoIPIntegration.Checked = Ayarlar.EnableNoIPUpdater;
            txtNoIPHost.Text = Ayarlar.NoIPHost;
            txtNoIPUser.Text = Ayarlar.NoIPUsername;
            txtNoIPPass.Text = Ayarlar.NoIPPassword;
        }

        private ushort GetPortSafe()
        {
            var portValue = ncPort.Value.ToString(CultureInfo.InvariantCulture);
            ushort port;
            return (!ushort.TryParse(portValue, out port)) ? (ushort)0 : port;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            ushort port = GetPortSafe();
            string password = txtPassword.Text;

            if (port == 0)
            {
                MessageBox.Show("Lütfen 0'dan büyük geçerli bir port giriniz..", "Lütfen geçerli bir port giriniz.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("Lütfen en az 3 karakterden oluşan geçerli ve güvenilir bir şifre kullanınız.",
                    "Güvenli bir şifre giriniz.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnListen.Text == "Dinlemeye Başla" && !_listenServer.Listening)
            {
                try
                {
                    AES.SetDefaultKey(password);

                    if (chkUseUpnp.Checked)
                    {
                        if (!UPnP.IsDeviceFound)
                        {
                            MessageBox.Show("Kullanılabilir hiçbir UPnP Cihazı Tespit Edilemedi!", "UPnP Cihazı Tespit Edilemedi", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            int outPort;
                            UPnP.CreatePortMap(port, out outPort);
                            if (port != outPort)
                            {
                                MessageBox.Show("UPnP Cihazıyla Bir Port Map Oluşturma Başarısız.!\nLütfen cihazınızın UPnP Port Map desteği olup olmadığına bakınız.", "Port map oluşturumu başarısız", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                        }
                    }
                    if (chkNoIPIntegration.Checked)
                        NoIpUpdater.Start();
                    _listenServer.Listen(port);
                }
                finally
                {
                    btnListen.Text = "Dinlemeyi Durdur";
                    ncPort.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
            else if (btnListen.Text == "Dinlemeyi Durdur" && _listenServer.Listening)
            {
                try
                {
                    _listenServer.Disconnect();
                    UPnP.DeletePortMap(port);
                }
                finally
                {
                    btnListen.Text = "Dinlemeye Başla";
                    ncPort.Enabled = true;
                    txtPassword.Enabled = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ushort port = GetPortSafe();
            string password = txtPassword.Text;

            if (port == 0)
            {
                MessageBox.Show("Lütfen 0'dan büyük geçerli bir port giriniz..", "Lütfen geçerli bir port giriniz.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("Lütfen en az 3 karakterden oluşan geçerli ve güvenilir bir şifre kullanınız.",
                    "Güvenli bir şifre giriniz.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Ayarlar.ListenPort = port;
            Ayarlar.AutoListen = chkAutoListen.Checked;
            Ayarlar.ShowPopup = chkPopup.Checked;
            if (password != Ayarlar.Password)
                AES.SetDefaultKey(password);
            Ayarlar.Password = password;
            Ayarlar.UseUPnP = chkUseUpnp.Checked;
            Ayarlar.ShowToolTip = chkShowTooltip.Checked;
            Ayarlar.EnableNoIPUpdater = chkNoIPIntegration.Checked;
            Ayarlar.NoIPHost = txtNoIPHost.Text;
            Ayarlar.NoIPUsername = txtNoIPUser.Text;
            Ayarlar.NoIPPassword = txtNoIPPass.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Değişikliklerinizi iptal etmek istiyor musunuz?", "İptal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                this.Close();
        }
        private void chkNoIPIntegration_CheckedChanged(object sender, EventArgs e)
        {
            NoIPControlHandler(chkNoIPIntegration.Checked);
        }

        private void NoIPControlHandler(bool enable)
        {
            lblHost.Enabled = enable;
            lblUser.Enabled = enable;
            lblPass.Enabled = enable;
            txtNoIPHost.Enabled = enable;
            txtNoIPUser.Enabled = enable;
            txtNoIPPass.Enabled = enable;
            chkShowPassword.Enabled = enable;
        }

        private void ShowPassword(bool show = true)
        {
            txtNoIPPass.PasswordChar = (show) ? (char)0 : (char)'●';
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            ShowPassword(chkShowPassword.Checked);
        }

        private void ncPort_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}