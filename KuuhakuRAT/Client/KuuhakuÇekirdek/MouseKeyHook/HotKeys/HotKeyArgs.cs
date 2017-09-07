using System;
namespace xClient.KuuhakuÇekirdek.MouseKeyHook.HotKeys
{
    public sealed class HotKeyArgs : EventArgs
    {
        private readonly DateTime m_TimeOfExecution;

        public HotKeyArgs(DateTime triggeredAt)
        {
            m_TimeOfExecution = triggeredAt;
        }

        public DateTime Time
        {
            get { return m_TimeOfExecution; }
        }
    }
}