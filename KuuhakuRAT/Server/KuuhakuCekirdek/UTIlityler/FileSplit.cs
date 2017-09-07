using System;
using System.IO;

namespace xServer.KuuhakuCekirdek.UTIlityler
{
    public class FileSplit
    {
        private const int MAX_BLOCK_SIZE = 65535;
        private readonly object _fileStreamLock = new object();
        private int _maxBlocks;

        public FileSplit(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
        public string LastError { get; private set; }

        public int MaxBlocks
        {
            get
            {
                if (_maxBlocks > 0 || _maxBlocks == -1)
                    return _maxBlocks;
                try
                {
                    FileInfo fInfo = new FileInfo(Path);

                    if (!fInfo.Exists)
                        throw new FileNotFoundException();

                    _maxBlocks = (int)Math.Ceiling(fInfo.Length / (double)MAX_BLOCK_SIZE);
                }
                catch (UnauthorizedAccessException)
                {
                    _maxBlocks = -1;
                    LastError = "Erişim reddedildi";
                }
                catch (IOException ex)
                {
                    _maxBlocks = -1;

                    if (ex is FileNotFoundException)
                        LastError = "Dosya bulunamadı";
                    if (ex is PathTooLongException)
                        LastError = "Dizin çok uzun";
                }

                return _maxBlocks;
            }
        }

        private int GetSize(long length)
        {
            return (length < MAX_BLOCK_SIZE) ? (int)length : MAX_BLOCK_SIZE;
        }

        public bool ReadBlock(int blockNumber, out byte[] readBytes)
        {
            try
            {
                if (blockNumber > MaxBlocks)
                    throw new ArgumentOutOfRangeException();

                lock (_fileStreamLock)
                {
                    using (FileStream fStream = File.OpenRead(Path))
                    {
                        if (blockNumber == 0)
                        {
                            fStream.Seek(0, SeekOrigin.Begin);
                            var length = fStream.Length - fStream.Position;
                            if (length < 0)
                                throw new IOException("negatif uzunluk");
                            readBytes = new byte[GetSize(length)];
                            fStream.Read(readBytes, 0, readBytes.Length);
                        }
                        else
                        {
                            fStream.Seek(blockNumber * MAX_BLOCK_SIZE, SeekOrigin.Begin);
                            var length = fStream.Length - fStream.Position;
                            if (length < 0)
                                throw new IOException("negatif uzunluk");
                            readBytes = new byte[GetSize(length)];
                            fStream.Read(readBytes, 0, readBytes.Length);
                        }
                    }
                }

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                readBytes = new byte[0];
                LastError = "Blok Sayısı, Max Blokdan büyük";
            }
            catch (UnauthorizedAccessException)
            {
                readBytes = new byte[0];
                LastError = "Erişim reddedildi";
            }
            catch (IOException ex)
            {
                readBytes = new byte[0];

                if (ex is FileNotFoundException)
                    LastError = "Dosya bulunamadı";
                else if (ex is DirectoryNotFoundException)
                    LastError = "Dizin bulunamadı";
                else if (ex is PathTooLongException)
                    LastError = "Dizin çok uzun";
                else
                    LastError = "Dosya akışından okuma hatası";
            }

            return false;
        }

        public bool AppendBlock(byte[] block, int blockNumber)
        {
            try
            {
                if (!File.Exists(Path) && blockNumber > 0)
                    throw new FileNotFoundException();

                lock (_fileStreamLock)
                {
                    if (blockNumber == 0)
                    {
                        using (FileStream fStream = File.Open(Path, FileMode.Create, FileAccess.Write))
                        {
                            fStream.Seek(0, SeekOrigin.Begin);
                            fStream.Write(block, 0, block.Length);
                        }

                        return true;
                    }

                    using (FileStream fStream = File.Open(Path, FileMode.Append, FileAccess.Write))
                    {
                        fStream.Seek(blockNumber * MAX_BLOCK_SIZE, SeekOrigin.Begin);
                        fStream.Write(block, 0, block.Length);
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                LastError = "Erişim Hatası";
            }
            catch (IOException ex)
            {
                if (ex is FileNotFoundException)
                    LastError = "Dosya Bulunamadı";
                else if (ex is DirectoryNotFoundException)
                    LastError = "Dizin Bulunamadı";
                else if (ex is PathTooLongException)
                    LastError = "Dizin Çok Uzun";
                else
                    LastError = "Dosya akımına yazma başarısız";
            }

            return false;
        }
    }
}