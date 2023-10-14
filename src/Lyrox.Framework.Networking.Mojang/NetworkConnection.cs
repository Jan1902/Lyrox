using System.Net.Sockets;
using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Core.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.Networking.Mojang.Data.Types;
using Lyrox.Framework.Shared.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Lyrox.Framework.Networking.Mojang;

public class NetworkConnection : INetworkConnection
{
    public event EventHandler<(int PacketID, byte[] Data)> NetworkPacketReceived;

    private readonly ILyroxConfiguration _lyroxConfiguration;
    private readonly ILogger<NetworkConnection> _logger;
    private readonly IMessageBus _messageBus;
    private readonly IPacketSerializer _packetSerializer;

    private readonly RecyclableMemoryStreamManager _streamManager;

    private readonly Socket _socket;

    private int _currentQueuePosition = 0;
    private readonly byte[] _buffer;
    private readonly byte[] _dataQueue;

    private bool _compressionEnabled;

    public NetworkConnection(ILyroxConfiguration lyroxConfiguration, ILogger<NetworkConnection> logger, IMessageBus messageBus, IPacketSerializer packetSerializer)
    {
        _streamManager = new();
        _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _buffer = new byte[16384];
        _dataQueue = new byte[262144];

        _lyroxConfiguration = lyroxConfiguration;
        _logger = logger;
        _messageBus = messageBus;
        _packetSerializer = packetSerializer;
    }

    public async Task ConnectAsync()
    {
        if (_lyroxConfiguration.IPAdress == default
            || _lyroxConfiguration.Port == default)
            throw new ArgumentException("Invalid Connection Information specified in Lyrox Configuration!");

        try
        {
            await _socket.ConnectAsync(_lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
            _logger.LogInformation("Connected to Server at {host}: {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
            await _messageBus.PublishAsync(new ConnectionEstablishedMessage());
            _socket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error connecting to Host {host} at Port {port}", _lyroxConfiguration.IPAdress, _lyroxConfiguration.Port);
            throw;
        }
    }

    public async Task SendPacketAsync(IPacket packet)
    {
        if (!_socket.Connected)
            return;

        using var stream = _streamManager.GetStream();
        using var writer = new MojangBinaryWriter(stream);

        var opCode = _packetSerializer.GetOpCode(packet.GetType())
            ?? throw new Exception("Invalid Packet Type");

        var data = _packetSerializer.SerializePacket(packet.GetType(), packet);

        writer.WriteVarInt(opCode.ToBytesAsVarInt().Length + data.Length);
        writer.WriteVarInt(opCode);

        stream.Write(data);

        try
        {
            await _socket.SendAsync(stream.ToArray(), SocketFlags.None);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending packet with OP Code {op} and size {size} to server", opCode, data.Length);
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
            {
                _messageBus.PublishAsync(new ConnectionTerminatedMessage());
                _logger.LogWarning("Connection to Server has been terminated");
            }
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
            using var reader = new MojangBinaryReader(stream);

            var packetLength = reader.ReadVarInt(); // This could be cached
            var totalLength = packetLength.ToBytesAsVarInt().Length + packetLength;

            if (_currentQueuePosition < totalLength)
                break;

            if (_compressionEnabled)
            {
                var dataLength = reader.ReadVarInt();
                if (dataLength != 0)
                    throw new NotImplementedException("Compression is not implemented yet!");
            }

            var opCode = reader.ReadVarInt();
            var data = new byte[packetLength - opCode.ToBytesAsVarInt().Length];
            stream.Read(data, 0, data.Length);

            NetworkPacketReceived?.Invoke(this, (opCode, data));

            Array.Copy(_dataQueue, totalLength, _dataQueue, 0, _currentQueuePosition - totalLength);
            _currentQueuePosition -= totalLength;
        }
    }
}
