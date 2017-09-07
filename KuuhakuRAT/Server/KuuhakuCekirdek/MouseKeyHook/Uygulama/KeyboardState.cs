using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.MouseKeyHook.WinApi;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama
{
    internal class KeyboardState
    {
        private readonly byte[] m_KeyboardStateNative;

        private KeyboardState(byte[] keyboardStateNative)
        {
            m_KeyboardStateNative = keyboardStateNative;
        }
        public static KeyboardState GetCurrent()
        {
            byte[] keyboardStateNative = new byte[256];
            KeyboardNativeMethods.GetKeyboardState(keyboardStateNative);
            return new KeyboardState(keyboardStateNative);
        }

        internal byte[] GetNativeState()
        {
            return m_KeyboardStateNative;
        }

        public bool IsDown(Keys key)
        {
            byte keyState = GetKeyState(key);
            bool isDown = GetHighBit(keyState);
            return isDown;
        }

        public bool IsToggled(Keys key)
        {
            byte keyState = GetKeyState(key);
            bool isToggled = GetLowBit(keyState);
            return isToggled;
        }

        public bool AreAllDown(IEnumerable<Keys> keys)
        {
            foreach (Keys key in keys)
            {
                if (!IsDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        private byte GetKeyState(Keys key)
        {
            int virtualKeyCode = (int) key;
            if (virtualKeyCode < 0 || virtualKeyCode > 255)
            {
                throw new ArgumentOutOfRangeException("key", key, "Değer 0 ile 255 arasında olmalıdır.");
            }
            return m_KeyboardStateNative[virtualKeyCode];
        }

        private static bool GetHighBit(byte value)
        {
            return (value >> 7) != 0;
        }

        private static bool GetLowBit(byte value)
        {
            return (value & 1) != 0;
        }
    }
}