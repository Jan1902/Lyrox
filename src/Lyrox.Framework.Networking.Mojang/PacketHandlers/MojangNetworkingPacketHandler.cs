using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang.Events;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;
using Lyrox.Framework.Networking.Mojang.Packets.ServerBound;
using Lyrox.Framework.Networking.Mojang.Types;
using Lyrox.Framework.Shared.Events;

namespace Lyrox.Framework.Networking.Mojang.PacketHandlers;

public class MojangNetworkingPacketHandler : IRawPacketHandler, IPacketHandler<KeepAliveCB>
{
    private readonly INetworkConnection _networkConnection;
    private readonly IMessageBus _messageBus;

    private readonly Dictionary<ProtocolState, Dictionary<int, Func<byte[], Task>>> _handlers;
    private ProtocolState _currentProtocolState;

    public MojangNetworkingPacketHandler(INetworkConnection networkConnection, IMessageBus messageBus)
    {
        _networkConnection = networkConnection;
        _messageBus = messageBus;

        _currentProtocolState = ProtocolState.Handshaking;
        _messageBus.Subscribe<ProtocolStateChangedMessage>(HandleProtocolStateChange);

        _handlers = new()
        {
            [ProtocolState.Login] = new()
            {
                [0x00] = HandleDisconnectLogin,
                [0x02] = HandleLoginSuccess
            }
        };
    }

    private Task HandleProtocolStateChange(ProtocolStateChangedMessage protocolStateChanged)
    {
        _currentProtocolState = protocolStateChanged.ProtocolState;
        return Task.CompletedTask;
    }

    public Task HandleRawPacketAsync(int opCode, byte[] data)
    {
        if (_handlers.TryGetValue(_currentProtocolState, out var stateHandlers)
            && stateHandlers.TryGetValue(opCode, out var handler))
            return handler.Invoke(data);

        return Task.CompletedTask;
    }

    private Task HandleLoginSuccess(byte[] data)
        => _messageBus.PublishAsync(new ProtocolStateChangedMessage(ProtocolState.Play));

    private Task HandleDisconnectLogin(byte[] data)
        => _messageBus.PublishAsync(new ConnectionTerminatedMessage());

    public Task HandlePacketAsync(KeepAliveCB networkPacket)
        => _networkConnection.SendPacket(new KeepAliveSB(networkPacket.KeepAliveID));
}
