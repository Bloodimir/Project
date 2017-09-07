using System;
using System.Windows.Forms;
using Microsoft.Win32;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.KayıtDefteri;

namespace xServer.Formlar
{
    public partial class FrmWordEdit : Form
    {
        private readonly Client _connectClient;
        private readonly string _keyPath;
        private readonly RegValueData _value;
        private int valueBase;

        public FrmWordEdit(string keyPath, RegValueData value, Client c)
        {
            _connectClient = c;
            _keyPath = keyPath;
            _value = value;

            InitializeComponent();

            valueNameTxtBox.Text = value.Name;

            if (value.Kind == RegistryValueKind.DWord)
            {
                Text = "(32-bit) DWORD Değerini Düzenle";
                valueDataTxtBox.Text = ((uint) (int) value.Data).ToString("X");
                valueDataTxtBox.MaxLength = HEXA_32BIT_MAX_LENGTH;
            }
            else if (value.Kind == RegistryValueKind.QWord)
            {
                Text = "(64-bit) QWORD Değerini Düzenle";
                valueDataTxtBox.Text = ((ulong) (long) value.Data).ToString("X");
                valueDataTxtBox.MaxLength = HEXA_64BIT_MAX_LENGTH;
            }
            valueBase = HEXA_BASE;
        }

        private void FrmRegValueEditWord_Load(object sender, EventArgs e)
        {
            valueDataTxtBox.Select();
            valueDataTxtBox.Focus();
        }

        private static bool IsHexa(char c)
        {
            return (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || char.IsDigit(c);
        }

        #region Sabit Değer

        private const int HEXA_32BIT_MAX_LENGTH = 8;
        private const int HEXA_64BIT_MAX_LENGTH = 16;
        private const int DEC_32BIT_MAX_LENGTH = 10;
        private const int DEC_64BIT_MAX_LENGTH = 20;
        private const int HEXA_BASE = 16;
        private const int DEC_BASE = 10;

        private const string DWORD_UYARI =
            "Girilen ondalık değer bir DWORD (32-bit sayının) alabileceği değerden daha büyük. Sayıyı biraz kırpmayı dene ;)";

        private const string QWORD_UYARI =
            "Girilen ondalık değer bir QWORD (64-bit sayının) alabileceği değerden daha büyük. Sayıyı biraz kırpmayı dene;)";

        #endregion

        #region Yardımcı Fonksiyonlar

        private string GetDataAsString(int type)
        {
            if (!string.IsNullOrEmpty(valueDataTxtBox.Text))
            {
                string text = valueDataTxtBox.Text;
                string returnType = (type == HEXA_BASE ? "X" : "D");
                try
                {
                    if (_value.Kind == RegistryValueKind.DWord)
                        return Convert.ToUInt32(text, valueBase).ToString(returnType);
                    return Convert.ToUInt64(text, valueBase).ToString(returnType);
                }
                catch
                {
                    string message = _value.Kind == RegistryValueKind.DWord ? DWORD_UYARI : QWORD_UYARI;
                    if (ShowWarning(message, "Taşma") == DialogResult.Yes)
                    {
                        if (_value.Kind == RegistryValueKind.DWord)
                            return uint.MaxValue.ToString(returnType);
                        return ulong.MaxValue.ToString(returnType);
                    }
                }
            }
            else
            {
                return "";
            }
            return null;
        }

        private DialogResult ShowWarning(string msg, string caption)
        {
            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        #endregion

        #region RadioButton Eylemleri

        private void radioHexa_Click(object sender, EventArgs e)
        {
            if (radioHexa.Checked)
            {
                string text = GetDataAsString(HEXA_BASE);
                if (text != null)
                {
                    valueDataTxtBox.MaxLength = HEXA_64BIT_MAX_LENGTH;

                    if (_value.Kind == RegistryValueKind.DWord)
                        valueDataTxtBox.MaxLength = HEXA_32BIT_MAX_LENGTH;

                    valueDataTxtBox.Text = text;
                    valueBase = HEXA_BASE;
                }
                else if (valueBase == DEC_BASE)
                {
                    radioDecimal.Checked = true;
                }
            }
        }

        private void radioDecimal_Click(object sender, EventArgs e)
        {
            if (radioDecimal.Checked)
            {
                string text = GetDataAsString(DEC_BASE);
                if (text != null)
                {
                    valueDataTxtBox.MaxLength = DEC_64BIT_MAX_LENGTH;

                    if (_value.Kind == RegistryValueKind.DWord)
                        valueDataTxtBox.MaxLength = DEC_32BIT_MAX_LENGTH;

                    valueDataTxtBox.Text = text;
                    valueBase = DEC_BASE;
                }
                else if (valueBase == HEXA_BASE)
                {
                    radioHexa.Checked = true;
                }
            }
        }

        #endregion

        #region Tamam ve İptal Tuşları

        private void okButton_Click(object sender, EventArgs e)
        {
            string text = GetDataAsString(DEC_BASE);
            if (text != null)
            {
                if (_value.Kind == RegistryValueKind.DWord)
                {
                    if (text != ((uint) (int) _value.Data).ToString())
                    {
                        uint unsignedValue = Convert.ToUInt32(text);
                        object valueData = (int) (unsignedValue);

                        new DoChangeRegistryValue(_keyPath, new RegValueData(_value.Name, _value.Kind, valueData))
                            .Execute(_connectClient);
                    }
                }
                else if (_value.Kind == RegistryValueKind.QWord)
                {
                    if (text != ((ulong) (long) _value.Data).ToString())
                    {
                        ulong unsignedValue = Convert.ToUInt64(text);
                        object valueData = (long) (unsignedValue);

                        new DoChangeRegistryValue(_keyPath, new RegValueData(_value.Name, _value.Kind, valueData))
                            .Execute(_connectClient);
                    }
                }
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void valueDataTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                if (radioHexa.Checked)
                {
                    e.Handled = !(IsHexa(e.KeyChar));
                }
                else
                {
                    e.Handled = !(char.IsDigit(e.KeyChar));
                }
            }
        }

        #endregion

        private void radioHexa_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}