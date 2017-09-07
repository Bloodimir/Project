using System.Runtime.Serialization;

namespace xClient.KuuhakuÇekirdek.Veri
{
    [DataContract]
    public class CoğrafikBilgi
    {
        [DataMember]
        public double Boylam { get; set; }

        [DataMember]
        public double Enlem { get; set; }

        [DataMember]
        public string Asn { get; set; }

        [DataMember]
        public string Denge { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public string bölge_Kodu { get; set; }

        [DataMember]
        public string Kıta_Kodu { get; set; }

        [DataMember]
        public string Dma_Kodu { get; set; }

        [DataMember]
        public string Şehir { get; set; }

        [DataMember]
        public string SaatDilimi { get; set; }

        [DataMember]
        public string Bölge { get; set; }

        [DataMember]
        public string Ülke_Kodu { get; set; }

        [DataMember]
        public string Isp { get; set; }

        [DataMember]
        public string Posta_Kodu { get; set; }

        [DataMember]
        public string Ülke { get; set; }

        [DataMember]
        public string Ülke_Kodu3 { get; set; }

        [DataMember]
        public string Bölge_Kodu { get; set; }
    }
}