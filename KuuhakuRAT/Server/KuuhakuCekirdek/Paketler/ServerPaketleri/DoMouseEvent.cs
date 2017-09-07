using System;
using xServer.KuuhakuCekirdek.Ağ;
using xServer.Enumlar2;

namespace xServer.KuuhakuCekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoMouseEvent : IPacket
    {
        public FareEylemleri Action { get; set; }

        public bool IsMouseDown { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int MonitorIndex { get; set; }

        public DoMouseEvent()
        {
        }

        public DoMouseEvent(FareEylemleri action, bool isMouseDown, int x, int y, int monitorIndex)
        {
            this.Action = action;
            this.IsMouseDown = isMouseDown;
            this.X = x;
            this.Y = y;
            this.MonitorIndex = monitorIndex;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}