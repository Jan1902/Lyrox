using System.Net.Sockets;
using Lyrox.Core.Configuration;
using Lyrox.Core.Events.Abstraction;
using Lyrox.Core.Events.Implementations;
using Lyrox.Networking.Packets;
using Lyrox.Networking.Packets.ServerBound;
using Lyrox.Networking.Types;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Lyrox.Networking.Connection
{
    internal class NetworkConnection : INetworkConnection
    {
        private readonly LyroxConfiguration _lyroxConfiguration;
        private readonly ILogger<NetworkConnection> _logger;
        private readonly IEventManager _eventManager;
        private readonly IPacketHandler _packetHandler;

        private readonly RecyclableMemoryStreamManager _streamManager;

        private readonly Socket _socket;

        private int _currentQueuePosition = 0;
        private readonly byte[] _buffer;
        private readonly byte[] _dataQueue;

        private bool _compressionEnabled;

        public NetworkConnection(LyroxConfiguration lyroxConfiguration, ILogger<NetworkConnection> logger, IEventManager eventManager, IPacketHandler packetHandler)
        {
            _streamManager = new();
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[16384];
            _dataQueue = new byte[262144];

            _lyroxConfiguration = lyroxConfiguration;
            _logger = logger;
            _eventManager = eventManager;
            _packetHandler = packetHandler;
        }

        public async Task Connect()
        {
            if (_lyroxConfiguration.IPAdress == default
                || _lyroxConfiguration.Port == default)
                throw new ArgumentException("Invalid Connection Information specified in Lyrox Configuration!");

            try
            {
                await _socket.ConnectAsync(_lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
                _logger.LogInformation("Connected to Server at {host}: {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
                _eventManager.PublishEvent(new ConnectionEstablishedEvent());
                _socket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error connecting to Host {host} at Port {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
                throw;
            }
        }

        public async Task SendPacket(ServerBoundPacket packet)
        {
            using var stream = _streamManager.GetStream();
            var data = packet.Build();
            stream.WriteVarInt(packet.OPCode.ToBytesAsVarInt().Length + data.Length);
            stream.WriteVarInt(packet.OPCode);
            stream.Write(data);

            try
            {
                await _socket.SendAsync(stream.ToArray(), SocketFlags.None);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error sending packet with OP Code {op} and size {size} to server", packet.OPCode, data.Length);
                throw;
            }
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
                    _socket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                }
                else
                    _logger.LogWarning("Connection to Server has been terminated");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error receiving Data from Server");
                throw;
            }
        }

        private void CheckForCompletePackets()
        {
            while (_currentQueuePosition > 0)
            {
                using var stream = _streamManager.GetStream(_dataQueue);
                var packetLength = VarInt.ReadVarInt(stream); // This could be cached
                var totalLength = packetLength.ToBytesAsVarInt().Length + packetLength;

                if (_currentQueuePosition < totalLength)
                    break;

                if (_compressionEnabled)
                {
                    var dataLength = VarInt.ReadVarInt(stream);
                    if (dataLength != 0)
                        throw new NotImplementedException("Compression is not implemented yet!");
                }

                var opCode = VarInt.ReadVarInt(stream);
                var data = new byte[packetLength - opCode.ToBytesAsVarInt().Length];
                stream.Read(data, 0, data.Length);

                _packetHandler.HandlePacket(opCode, data);

                Array.Copy(_dataQueue, totalLength, _dataQueue, 0, _currentQueuePosition - totalLength);
                _currentQueuePosition -= totalLength;
            }
        }
    }
}
