using System.Collections.Generic;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.HotKeys
{
    public sealed class HotKeySetCollection : List<HotKeySeti>
    {
        private KeyChainHandler m_keyChain;

        public new void Add(HotKeySeti hks)
        {
            m_keyChain += hks.OnKey;
            base.Add(hks);
        }
        public new void Remove(HotKeySeti hks)
        {
            m_keyChain -= hks.OnKey;
            base.Remove(hks);
        }
        internal void OnKey(EventArgsExtKlavye e)
        {
            if (m_keyChain != null)
                m_keyChain(e);
        }

        private delegate void KeyChainHandler(EventArgsExtKlavye kex);
    }
}