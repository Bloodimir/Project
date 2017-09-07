using System.Windows.Forms;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.KuuhakuCekirdek.Eklentiler
{
    public static class ListeGörünümüEklentisi
    {
        private const uint SET_COLUMN_WIDTH = 4126;
        private const int AUTOSIZE_USEHEADER = -2;

        public static void AutosizeColumns(this ListView targetListView)
        {
            if (PlatformYardımcısı.MonodaÇalışıyor) return;
            for (int lngColumn = 0; lngColumn <= (targetListView.Columns.Count - 1); lngColumn++)
            {
                NativeMethods.SendMessage(targetListView.Handle, SET_COLUMN_WIDTH, lngColumn, AUTOSIZE_USEHEADER);
            }
        }

        public static void SelectAllItems(this ListView targetListView)
        {
            NativeMethodsHelper.SetItemState(targetListView.Handle, -1, 2, 2);
        }
    }
}