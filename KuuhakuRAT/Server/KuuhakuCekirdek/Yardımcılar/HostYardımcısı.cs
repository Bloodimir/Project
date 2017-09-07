using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.UTIlityler;

namespace xServer.KuuhakuCekirdek.Yardımcılar
{
    public static class HostYardımcısı
    {
        public static List<Sunucu> GetHostsList(string rawHosts)
        {
            List<Sunucu> hostsList = new List<Sunucu>();

            if (string.IsNullOrEmpty(rawHosts)) return hostsList;

            var hosts = rawHosts.Split(';');

            foreach (var hostPart in from host in hosts where (!string.IsNullOrEmpty(host) && host.Contains(':')) select host.Split(':'))
            {
                if (hostPart.Length != 2 || hostPart[0].Length < 1 || hostPart[1].Length < 1) continue; 

                ushort port;
                if (!ushort.TryParse(hostPart[1], out port)) continue;

                hostsList.Add(new Sunucu { Hostname = hostPart[0], Port = port });
            }

            return hostsList;
        }

        public static string GetRawHosts(List<Sunucu> hosts)
        {
            StringBuilder rawHosts = new StringBuilder();

            foreach (var host in hosts)
                rawHosts.Append(host + ";");

            return rawHosts.ToString();
        }

        public static string GetRawHosts(BindingList<Sunucu> hosts)
        {
            StringBuilder rawHosts = new StringBuilder();

            foreach (var host in hosts)
                rawHosts.Append(host + ";");

            return rawHosts.ToString();
        }
    }
}
