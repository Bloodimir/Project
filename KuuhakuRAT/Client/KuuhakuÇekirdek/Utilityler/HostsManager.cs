using System.Collections.Generic;
using xClient.KuuhakuÇekirdek.Veri;

namespace xClient.KuuhakuÇekirdek.Utilityler
{
    public class HostsManager
    {
        public bool IsEmpty { get { return _hosts.Count == 0; } }

        private readonly Queue<Host> _hosts = new Queue<Host>();

        public HostsManager(List<Host> hosts)
        {
            foreach(var host in hosts)
                _hosts.Enqueue(host);
        }

        public Host GetNextHost()
        {
            var temp = _hosts.Dequeue();
            _hosts.Enqueue(temp);

            return temp;
        }
    }
}
