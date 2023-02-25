using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

public interface IRawPacketHandler
    : IMessageHandler<RawPacketReceivedMessage>
{
    Task IMessageHandler<RawPacketReceivedMessage>
        .HandleMessageAsync(RawPacketReceivedMessage message)
        => HandleRawPacketAsync(message.OpCode, message.Data);

    Task HandleRawPacketAsync(int opCode, byte[] data);
}
