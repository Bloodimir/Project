using System.Windows.Forms;

namespace xServer.KuuhakuCekirdek.MouseKeyHook
{
    public interface KlavyeEylemleri
    {
        event KeyEventHandler KeyDown;

        event KeyPressEventHandler KeyPress;

        event KeyEventHandler KeyUp;
    }
}