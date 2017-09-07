using System.Windows.Forms;

namespace xServer.KuuhakuCekirdek.MouseKeyHook.Uygulama
{
    internal class TuşSeti
    {
        private MouseButtons m_Set;

        public TuşSeti()
        {
            m_Set = MouseButtons.None;
        }

        public void Add(MouseButtons element)
        {
            m_Set |= element;
        }

        public void Kaldır(MouseButtons element)
        {
            m_Set &= ~element;
        }

        public bool İçerir(MouseButtons element)
        {
            return (m_Set & element) != MouseButtons.None;
        }
    }
}