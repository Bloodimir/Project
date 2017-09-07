using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using xServer.KuuhakuCekirdek.Veri;
using xServer.KuuhakuCekirdek.NetSerializer;
using xServer.KuuhakuCekirdek.Ağ.UtilitylerAğ;
using xServer.KuuhakuCekirdek.Paketler;

namespace xServer.KuuhakuCekirdek.Ağ
{
    public class Server
    {

        public event ServerStateEventHandler ServerState;

        public delegate void ServerStateEventHandler(Server s, bool listening, ushort port);

        private void OnServerState(bool listening)
        {
            if (Listening == listening) return;

            Listening = listening;

            var handler = ServerState;
            if (handler != null)
            {
                handler(this, listening, Port);
            }
        }
        public event ClientStateEventHandler ClientState;
        public delegate void ClientStateEventHandler(Server s, Client c, bool connected);

        private void OnClientState(Client c, bool connected)
        {
            var handler = ClientState;

            if (!connected)
                RemoveClient(c);

            if (handler != null)
            {
                handler(this, c, connected);
            }
        }

        public event ClientReadEventHandler ClientRead;

        public delegate void ClientReadEventHandler(Server s, Client c, IPacket packet);
        private void OnClientRead(Client c, IPacket packet)
        {
            var handler = ClientRead;
            if (handler != null)
            {
                handler(this, c, packet);
            }
        }

        public event ClientWriteEventHandler ClientWrite;
        public delegate void ClientWriteEventHandler(Server s, Client c, IPacket packet, long length, byte[] rawData);
        private void OnClientWrite(Client c, IPacket packet, long length, byte[] rawData)
        {
            var handler = ClientWrite;
            if (handler != null)
            {
                handler(this, c, packet, length, rawData);
            }
        }
        public ushort Port { get; private set; }

        public long BytesReceived { get; set; }

        public long BytesSent { get; set; }

        public int BUFFER_SIZE { get { return 1024 * 16; } }

        public uint KEEP_ALIVE_TIME { get { return 25000; } }

        public uint KEEP_ALIVE_INTERVAL { get { return 25000; } }

        public int HEADER_SIZE { get { return 4; } }
        public int MAX_PACKET_SIZE { get { return (1024 * 1024) * 5; } }

        public PooledBufferManager BufferManager { get; private set; }
        public bool Listening { get; private set; }

        protected Client[] Clients
        {
            get
            {
                lock (_clientsLock)
                {
                    return _clients.ToArray();
                }
            }
        }

        public Serializer Serializer { get; protected set; }

        private Socket _handle;

        private SocketAsyncEventArgs _item;

        private List<Client> _clients;
        private readonly object _clientsLock = new object();

        protected bool ProcessingDisconnect { get; set; }

        protected Server()
        {
            _clients = new List<Client>();
            BufferManager = new PooledBufferManager(BUFFER_SIZE, 1) { ClearOnReturn = false };
        }
        public void Listen(ushort port)
        {
            this.Port = port;
            try
            {
                if (!Listening)
                {
                    if (_handle != null)
                    {
                        _handle.Close();
                        _handle = null;
                    }

                    _handle = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _handle.Bind(new IPEndPoint(IPAddress.Any, port));
                    _handle.Listen(1000);

                    ProcessingDisconnect = false;

                    OnServerState(true);

                    if (_item != null)
                    {
                        _item.Dispose();
                        _item = null;
                    }

                    _item = new SocketAsyncEventArgs();
                    _item.Completed += AcceptClient;

                    if (!_handle.AcceptAsync(_item))
                        AcceptClient(null, _item);
                }
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10048)
                {
                    MessageBox.Show("Port zaten kullanımda.", "Dinleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(
                        string.Format(
                            "Bilinmeyen bir hata oluştu: {0}\n\nHata Kodu: {1}\n\nLütfen en yakın zamanda hatayı buradan bana bildiriniz.:\n{2}",
                            ex.Message, ex.ErrorCode, Ayarlar.HelpURL), "Bilinmeyen Dinleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Disconnect();
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private void AcceptClient(object s, SocketAsyncEventArgs e)
        {
            try
            {
                do
                {
                    switch (e.SocketError)
                    {
                        case SocketError.Success:
                            if (BufferManager.BuffersAvailable == 0)
                                BufferManager.IncreaseBufferCount(1);

                            Client client = new Client(this, e.AcceptSocket);
                            AddClient(client);
                            OnClientState(client, true);
                            break;
                        case SocketError.ConnectionReset:
                            break;
                        default:
                            throw new Exception("SocketError");
                    }

                    e.AcceptSocket = null; 
                } while (!_handle.AcceptAsync(e));
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception)
            {
                Disconnect();
            }
        }
        private void AddClient(Client client)
        {
            lock (_clientsLock)
            {
                client.ClientState += OnClientState;
                client.ClientRead += OnClientRead;
                client.ClientWrite += OnClientWrite;
                _clients.Add(client);
            }
        }

        private void RemoveClient(Client client)
        {
            if (ProcessingDisconnect) return;

            lock (_clientsLock)
            {
                client.ClientState -= OnClientState;
                client.ClientRead -= OnClientRead;
                client.ClientWrite -= OnClientWrite;
                _clients.Remove(client);
            }
        }



        public void Disconnect()
        {
            if (ProcessingDisconnect) return;
            ProcessingDisconnect = true;

            if (_handle != null)
            {
                _handle.Close();
                _handle = null;
            }

            if (_item != null)
            {
                _item.Dispose();
                _item = null;
            }

            lock (_clientsLock)
            {
                while (_clients.Count != 0)
                {
                    try
                    {
                        _clients[0].Disconnect();
                        _clients[0].ClientState -= OnClientState;
                        _clients[0].ClientRead -= OnClientRead;
                        _clients[0].ClientWrite -= OnClientWrite;
                        _clients.RemoveAt(0);
                    }
                    catch
                    {
                    }
                }
            }

            ProcessingDisconnect = false;
            OnServerState(false);
        }
    }
}