using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xClient.KuuhakuÇekirdek.MouseKeyHook;
using xClient.KuuhakuÇekirdek.MouseKeyHook.HotKeys;
using xClient.KuuhakuÇekirdek.MouseKeyHook.Implementation;

namespace xServer.KuuhakuÇekirdek.MouseKeyHook.HotKeys
{
    public class HotKeySeti
    {
        public delegate void HotKeyHandler(object sender, HotKeyArgs e);

        private readonly IEnumerable<Keys> m_hotkeys;
        private readonly Dictionary<Keys, bool> m_hotkeystate;
        private readonly Dictionary<Keys, Keys> m_remapping;
        private bool m_enabled = true;
        private int m_hotkeydowncount;
        private int m_remappingCount;

        public HotKeySeti(IEnumerable<Keys> hotkeys)
        {
            m_hotkeystate = new Dictionary<Keys, bool>();
            m_remapping = new Dictionary<Keys, Keys>();
            m_hotkeys = hotkeys;
            InitializeKeys();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Keys> HotKeys
        {
            get { return m_hotkeys; }
        }

        public bool HotKeysActivated
        {
            get { return m_hotkeydowncount == (m_hotkeystate.Count - m_remappingCount); }
        }

        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                if (value)
                    InitializeKeys();

                m_enabled = value;
            }
        }

        public event HotKeyHandler OnHotKeysDownHold;
        public event HotKeyHandler OnHotKeysUp;
        public event HotKeyHandler OnHotKeysDownOnce;

        private void InvokeHotKeyHandler(HotKeyHandler hotKeyDelegate)
        {
            if (hotKeyDelegate != null)
                hotKeyDelegate(this, new HotKeyArgs(DateTime.Now));
        }

        private void InitializeKeys()
        {
            foreach (Keys k in HotKeys)
            {
                if (m_hotkeystate.ContainsKey(k))
                    m_hotkeystate.Add(k, false);

                m_hotkeystate[k] = KeyboardState.GetCurrent().IsDown(k);
            }
        }

        public bool UnregisterExclusiveOrKey(Keys anyKeyInTheExclusiveOrSet)
        {
            Keys primaryKey = GetExclusiveOrPrimaryKey(anyKeyInTheExclusiveOrSet);

            if (primaryKey == Keys.None || !m_remapping.ContainsValue(primaryKey))
                return false;

            List<Keys> keystoremove = new List<Keys>();

            foreach (KeyValuePair<Keys, Keys> pair in m_remapping)
            {
                if (pair.Value == primaryKey)
                    keystoremove.Add(pair.Key);
            }

            foreach (Keys k in keystoremove)
                m_remapping.Remove(k);

            --m_remappingCount;

            return true;
        }

        public Keys RegisterExclusiveOrKey(IEnumerable<Keys> orKeySet)
        {
            foreach (Keys k in orKeySet)
            {
                if (!m_hotkeystate.ContainsKey(k))
                    return Keys.None;
            }

            int i = 0;
            Keys primaryKey = Keys.None;

            foreach (Keys k in orKeySet)
            {
                if (i == 0)
                    primaryKey = k;

                m_remapping[k] = primaryKey;

                ++i;
            }

            ++m_remappingCount;

            return primaryKey;
        }

        private Keys GetExclusiveOrPrimaryKey(Keys k)
        {
            return (m_remapping.ContainsKey(k) ? m_remapping[k] : Keys.None);
        }

        private Keys GetPrimaryKey(Keys k)
        {
            return (m_remapping.ContainsKey(k) ? m_remapping[k] : k);
        }

        internal void OnKey(EventArgsExtKlavye kex)
        {
            if (!Enabled)
                return;
            Keys primaryKey = GetPrimaryKey(kex.KeyCode);

            if (kex.IsKeyDown)
                OnKeyDown(primaryKey);
            else
                OnKeyUp(primaryKey);
        }

        private void OnKeyDown(Keys k)
        {
            if (HotKeysActivated)
                InvokeHotKeyHandler(OnHotKeysDownHold);

            else if (m_hotkeystate.ContainsKey(k) && !m_hotkeystate[k])
            {
                m_hotkeystate[k] = true;
                ++m_hotkeydowncount;

                if (HotKeysActivated)
                    InvokeHotKeyHandler(OnHotKeysDownOnce);
            }
        }

        private void OnKeyUp(Keys k)
        {
            if (m_hotkeystate.ContainsKey(k) && m_hotkeystate[k])
            {
                bool wasActive = HotKeysActivated;

                m_hotkeystate[k] = false;
                --m_hotkeydowncount;
                if (wasActive)
                    InvokeHotKeyHandler(OnHotKeysUp);
            }
        }
    }
}