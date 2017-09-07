using System;
using System.Windows.Forms;

namespace xClient.Kuuhaku«ekirdek.MouseKeyHook
{
    public interface FareEylemleri
    {
        event MouseEventHandler MouseMove;
        event EventHandler<EventArgsExtFare> MouseMoveExt;
        event MouseEventHandler MouseClick;
        event MouseEventHandler MouseDown;
        event EventHandler<EventArgsExtFare> MouseDownExt;
        event MouseEventHandler MouseUp;
        event EventHandler<EventArgsExtFare> MouseUpExt;
        event MouseEventHandler MouseWheel;
        event MouseEventHandler MouseDoubleClick;
    }
}