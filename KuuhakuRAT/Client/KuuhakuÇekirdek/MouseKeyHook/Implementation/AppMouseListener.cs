// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using xClient.Kuuhaku«ekirdek.MouseKeyHook.WinApi;

namespace xClient.Kuuhaku«ekirdek.MouseKeyHook.Implementation
{
    internal class AppMouseListener : MouseListener
    {
        public AppMouseListener()
            : base(HookHelper.HookAppMouse)
        {
        }

        protected override EventArgsExtFare GetEventArgs(CallbackData data)
        {
            return EventArgsExtFare.FromRawDataApp(data);
        }
    }
}