using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook
{
    public class EventArgsExtFare : MouseEventArgs
    {
        internal EventArgsExtFare(MouseButtons buttons, int clicks, Point point, int delta, int zamandamgasi,
            bool faredown, bool fareup)
            : base(buttons, clicks, point.X, point.Y, delta)
        {
            FareDown = faredown;
            FareUp = fareup;
            ZamanDamgası = zamandamgasi;
        }

        public bool İşlendi { get; set; }

        public bool TekerlekKaydırıldı
        {
            get { return Delta != 0; }
        }

        public bool Tıklandı
        {
            get { return Clicks > 0; }
        }

        public bool FareDown { get; private set; }
        public bool FareUp { get; private set; }
        public int ZamanDamgası { get; private set; }

        internal Point Point
        {
            get { return new Point(X, Y); }
        }

        internal static EventArgsExtFare FromRawDataApp(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;

            var marshalledMouseStruct =
                (AppMouseStruct) Marshal.PtrToStructure(lParam, typeof (AppMouseStruct));
            return FromRawDataUniversal(wParam, marshalledMouseStruct.ToMouseStruct());
        }

        internal static EventArgsExtFare FromRawDataGlobal(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;

            var marshalledMouseStruct = (MouseStruct) Marshal.PtrToStructure(lParam, typeof (MouseStruct));
            return FromRawDataUniversal(wParam, marshalledMouseStruct);
        }

        private static EventArgsExtFare FromRawDataUniversal(IntPtr wParam, MouseStruct mouseInfo)
        {
            var button = MouseButtons.None;
            short mouseDelta = 0;
            var clickCount = 0;

            var isMouseKeyDown = false;
            var isMouseKeyUp = false;


            switch ((long) wParam)
            {
                case Messages.WM_LBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;
                case Messages.WM_RBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;
                case Messages.WM_MBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Middle;
                    clickCount = 2;
                    break;
                case Messages.WM_MOUSEWHEEL:
                    mouseDelta = mouseInfo.MouseData;
                    break;
                case Messages.WM_XBUTTONDOWN:
                    button = mouseInfo.MouseData == 1
                        ? MouseButtons.XButton1
                        : MouseButtons.XButton2;
                    isMouseKeyDown = true;
                    clickCount = 1;
                    break;

                case Messages.WM_XBUTTONUP:
                    button = mouseInfo.MouseData == 1
                        ? MouseButtons.XButton1
                        : MouseButtons.XButton2;
                    isMouseKeyUp = true;
                    clickCount = 1;
                    break;

                case Messages.WM_XBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = mouseInfo.MouseData == 1
                        ? MouseButtons.XButton1
                        : MouseButtons.XButton2;
                    clickCount = 2;
                    break;

                case Messages.WM_MOUSEHWHEEL:
                    mouseDelta = mouseInfo.MouseData;
                    break;
            }
            // hentai
            var e = new EventArgsExtFare(
                button,
                clickCount,
                mouseInfo.Point,
                mouseDelta,
                mouseInfo.Timestamp,
                isMouseKeyDown,
                isMouseKeyUp);

            return e;
        }

        internal EventArgsExtFare ToDoubleClickEventArgs()
        {
            return new EventArgsExtFare(Button, 2, Point, Delta, ZamanDamgası, FareDown, FareUp);
        }
    }
}