using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xClient.KuuhakuÇekirdek.KayıtDefteri
{
    [Serializable]
    public class RegSeekerMatch
    {
        public string Key { get; private set; }
        public List<RegValueData> Data { get; private set; }
        public bool HasSubKeys { get; private set; }

        public RegSeekerMatch(string key, List<RegValueData> data, int subkeycount)
        {
            Key = key;
            Data = data;
            HasSubKeys = (subkeycount > 0);
        }

        public override string ToString()
        {
            return string.Format("({0}:{1})", Key, Data.ToString());
        }
    }
}
