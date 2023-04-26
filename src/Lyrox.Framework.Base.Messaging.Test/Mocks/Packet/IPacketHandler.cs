using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Base.Messaging.Test.Mocks.Packet;
internal interface IPacketHandler<TPacket> : IMessageHandler<PacketReceivedMessage<TPacket>>
{
}
