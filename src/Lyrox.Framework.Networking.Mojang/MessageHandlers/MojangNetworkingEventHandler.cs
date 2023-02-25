using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang.Messages;
using Lyrox.Framework.Networking.Mojang.Packets.ServerBound;
using Lyrox.Framework.Networking.Mojang.Types;
using Lyrox.Framework.Shared.Messages;

namespace Lyrox.Framework.Networking.Mojang.MessageHandlers;

public class MojangNetworkingMessageHandler
    : IMessageHandler<ConnectionEstablishedMessage>
{
    private readonly INetworkConnection _networkConnection;
    private readonly IMessageBus _messageBus;
    private readonly ILyroxConfiguration _configuration;

    public MojangNetworkingMessageHandler(INetworkConnection networkConnection, ILyroxConfiguration configuration, IMessageBus messageBus)
    {
        _networkConnection = networkConnection;
        _configuration = configuration;
        _messageBus = messageBus;
    }

    public async Task HandleMessageAsync(ConnectionEstablishedMessage message)
    {
        await _networkConnection.SendPacket(new Handshake(760, _configuration.IPAdress, (ushort)_configuration.Port, ProtocolState.Login));
        await _networkConnection.SendPacket(new LoginStart(_configuration.Username));
        await _messageBus.PublishAsync(new ProtocolStateChangedMessage(ProtocolState.Login));
    }
}
