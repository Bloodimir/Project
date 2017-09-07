using System.Collections.Generic;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama
{
    internal class AppKeyListener : KeyListener
    {
        public AppKeyListener()
            : base(HookHelper.HookAppKeyboard)
        {
        }

        protected override IEnumerable<EventArgsTu�Bas���> GetPressEventArgs(CallbackData data)
        {
            return EventArgsTu�Bas���.FromRawDataApp(data);
        }

        protected override EventArgsExtKlavye GetDownUpEventArgs(CallbackData data)
        {
            return EventArgsExtKlavye.FromRawDataApp(data);
        }
    }
}