using System;
using System.Collections.Generic;

namespace xServer.KuuhakuCekirdek.Ağ.UtilitylerAğ
{
    public class PooledBufferManager
    {
        private readonly int _bufferLength;
        private readonly Stack<byte[]> _buffers;

        #region constructor

        public PooledBufferManager(int baseBufferLength, int baseBufferCount)
        {
            if (baseBufferLength <= 0)
                throw new ArgumentOutOfRangeException("baseBufferLength", baseBufferLength,
                    "Buffer uzunluğu pozitif bir sayı olmalıdır.");
            if (baseBufferCount <= 0)
                throw new ArgumentOutOfRangeException("baseBufferCount", baseBufferCount,
                    "Buffer miktarı pozitif bir sayı olmalıdır.");

            _bufferLength = baseBufferLength;
            MaxBufferCount = baseBufferCount;

            _buffers = new Stack<byte[]>(baseBufferCount);

            for (int i = 0; i < baseBufferCount; i++)
            {
                _buffers.Push(new byte[baseBufferLength]);
            }
        }

        #endregion

        #region events

        public event EventHandler NewBufferAllocated;

        protected virtual void OnNewBufferAllocated(EventArgs e)
        {
            if (NewBufferAllocated != null)
                NewBufferAllocated(this, e);
        }

        public event EventHandler BufferRequested;

        protected virtual void OnBufferRequested(EventArgs e)
        {
            if (BufferRequested != null)
                BufferRequested(this, e);
        }

        public event EventHandler BufferReturned;

        protected virtual void OnBufferReturned(EventArgs e)
        {
            if (BufferReturned != null)
                BufferReturned(this, e);
        }

        #endregion

        #region properties

        public int BufferLength
        {
            get { return _bufferLength; }
        }

        public int MaxBufferCount { get; private set; }

        public int BuffersAvailable
        {
            get { return _buffers.Count; }
        }

        public bool ClearOnReturn { get; set; }

        #endregion

        #region methods

        public byte[] GetBuffer()
        {
            if (_buffers.Count > 0)
            {
                lock (_buffers)
                {
                    if (_buffers.Count > 0)
                    {
                        byte[] buffer = _buffers.Pop();
                        return buffer;
                    }
                }
            }

            return AllocateNewBuffer();
        }

        private byte[] AllocateNewBuffer()
        {
            byte[] newBuffer = new byte[_bufferLength];
            MaxBufferCount++;
            OnNewBufferAllocated(EventArgs.Empty);

            return newBuffer;
        }

        public bool ReturnBuffer(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length != _bufferLength)
                return false;

            if (ClearOnReturn)
            {
                for (int i = 0; i < _bufferLength; i++)
                {
                    buffer[i] = 0;
                }
            }

            lock (_buffers)
            {
                if (!_buffers.Contains(buffer))
                    _buffers.Push(buffer);
            }
            return true;
        }

        public void IncreaseBufferCount(int buffersToAdd)
        {
            if (buffersToAdd <= 0)
                throw new ArgumentOutOfRangeException("buffersToAdd", buffersToAdd,
                    "Eklenicek buffer sayısı negatif olmayan 0'dan büyük bir sayı olmak zorundadır.");

            List<byte[]> newBuffers = new List<byte[]>(buffersToAdd);
            for (int i = 0; i < buffersToAdd; i++)
            {
                newBuffers.Add(new byte[_bufferLength]);
            }

            lock (_buffers)
            {
                MaxBufferCount += buffersToAdd;
                for (int i = 0; i < buffersToAdd; i++)
                {
                    _buffers.Push(newBuffers[i]);
                }
            }
        }

        public int DecreaseBufferCount(int buffersToRemove)
        {
            if (buffersToRemove <= 0)
                throw new ArgumentOutOfRangeException("buffersToRemove", buffersToRemove,
                    "Kaldırılacak buffer sayısı negatif olmayan 0'dan büyük bir sayı olmak zorundadır.");

            var numRemoved = 0;

            lock (_buffers)
            {
                for (int i = 0; i < buffersToRemove && _buffers.Count > 0; i++)
                {
                    _buffers.Pop();
                    numRemoved++;
                }
            }

            return numRemoved;
        }

        #endregion
    }
}