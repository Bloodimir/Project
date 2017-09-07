using System;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.UTIlityler;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.Kontroller
{
    internal class AeroListView : ListView
    {
        private const uint WM_CHANGEUISTATE = 0x127;

        private const int UIS_SET = 1;
        private const int UISF_HIDEFOCUS = 0x1;

        private ListViewColumnSorter LvwColumnSorter { get; set; }

        public AeroListView()
            : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.LvwColumnSorter = new ListViewColumnSorter();
            this.ListViewItemSorter = LvwColumnSorter;
            this.View = View.Details;
            this.FullRowSelect = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (PlatformYardımcısı.MonodaÇalışıyor) return;

            if (PlatformYardımcısı.VistaYadaÜstü)
            {
                NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
            }

            if (PlatformYardımcısı.XpYadaÜstü)
            {
                NativeMethods.SendMessage(this.Handle, WM_CHANGEUISTATE,
                    NativeMethodsHelper.MakeLong(UIS_SET, UISF_HIDEFOCUS), 0);
            }
        }
        
        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            if (e.Column == this.LvwColumnSorter.SortColumn)
            {
                this.LvwColumnSorter.Order = (this.LvwColumnSorter.Order == SortOrder.Ascending)
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                this.LvwColumnSorter.SortColumn = e.Column;
                this.LvwColumnSorter.Order = SortOrder.Ascending;
            }
            this.Sort();
        }
    }
}