using System.Net.Sockets;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events;
using Lyrox.Core.Events.Implementations;
using Lyrox.Networking.Types;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Lyrox.Networking.Connection
{
    public class NetworkConnection : INetworkConnection
    {
        private readonly LyroxConfiguration _lyroxConfiguration;
        private readonly ILogger<NetworkConnection> _logger;
        private readonly IEventManager _eventManager;

        private readonly RecyclableMemoryStreamManager _streamManager;

        private readonly Socket _socket;

        private int _currentQueuePosition = 0;
        private readonly byte[] _buffer;
        private readonly byte[] _dataQueue;

        public NetworkConnection(LyroxConfiguration lyroxConfiguration, ILogger<NetworkConnection> logger, IEventManager eventManager)
        {
            _streamManager = new();
            _socket = new (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[4096];
            _dataQueue = new byte[24576];

            _lyroxConfiguration = lyroxConfiguration;
            _logger = logger;
            _eventManager = eventManager;
        }

        public async Task Connect()
        {
            if (_lyroxConfiguration.IPAdress == null
                || _lyroxConfiguration.Port == null)
                throw new ArgumentException("Invalid Connection Information specified in Lyrox Configuration!");

            try
            {
                await _socket.ConnectAsync(_lyroxConfiguration.IPAdress ?? string.Empty, _lyroxConfiguration.Port ?? 0);
                _eventManager.PublishEvent(new ConnectionEstablishedEvent());
                _logger.LogInformation("Connected to Server at {host}: {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
                _socket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error connecting to Host {host} at Port {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
                throw;
            }
        }

        public async Task SendPacket(int opCode, byte[] data)
        {
            using var stream = _streamManager.GetStream();
            stream.WriteVarInt(opCode.ToBytesAsVarInt().Length + data.Length);
            stream.WriteVarInt(opCode);
            stream.Write(data);

            await _socket.SendAsync(stream.ToArray(), SocketFlags.None);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var bytesRead = _socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    Array.Copy(_buffer, 0, _dataQueue, _currentQueuePosition, bytesRead);
                    _currentQueuePosition += bytesRead;
                    CheckForCompletePackets();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error receiving Data from Server");
                throw;
            }
            finally
            {
                _socket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }
        }

        private void CheckForCompletePackets()
        {
            while(_currentQueuePosition > 0)
            {
                using var stream = _streamManager.GetStream(_dataQueue);
                var packetLength = VarInt.ReadFromStream(stream);
                var totalLength = packetLength.ToBytesAsVarInt().Length + packetLength;

                if (_currentQueuePosition < totalLength)
                    break;

                var dataLength = VarInt.ReadFromStream(stream);
                if (dataLength != 0)
                    throw new NotImplementedException("Compression is not implemented yet!");

                var opCode = VarInt.ReadFromStream(stream);
                var data = new byte[packetLength - dataLength.ToBytesAsVarInt().Length - opCode.ToBytesAsVarInt().Length];
                stream.Read(data, 0, data.Length);

                HandlePacket(opCode, data);
                Array.Copy(_dataQueue, totalLength, _dataQueue, 0, _currentQueuePosition - totalLength);
                _currentQueuePosition -= totalLength;
            }
        }

        private void HandlePacket(int opCode, byte[] data)
        {
            
        }
    }
}
