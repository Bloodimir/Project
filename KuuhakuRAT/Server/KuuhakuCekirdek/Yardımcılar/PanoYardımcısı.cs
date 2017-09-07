using System;
using System.Windows.Forms;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class PanoYardımcısı
    {
        public static void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception)
            {
            }
        }
    }
}
