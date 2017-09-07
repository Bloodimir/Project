using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama
{
    internal class AppMouseListener : MouseListener
    {
        public AppMouseListener()
            : base(HookHelper.HookAppMouse)
        {
        }

        protected override EventArgsExtFare GetEventArgs(CallbackData data)
        {
            return EventArgsExtFare.FromRawDataApp(data);
        }
    }
}