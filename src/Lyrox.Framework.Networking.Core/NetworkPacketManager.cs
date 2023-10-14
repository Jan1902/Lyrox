using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core.Data.Abstraction;

namespace Lyrox.Framework.Networking.Core;

public class NetworkPacketManager : INetworkPacketManager
{
    private readonly PacketTypeMapping _packetMapping;
    private readonly IMessageBus _messageBus;
    private readonly IPacketSerializer _packetSerializer;
    private readonly IMinecraftBinaryReaderWriterFactory _readerWriterFactory;

    public NetworkPacketManager(PacketTypeMapping packetMapping, IMessageBus messageBus, IPacketSerializer packetSerializer, IMinecraftBinaryReaderWriterFactory readerWriterFactory)
    {
        _packetMapping = packetMapping;
        _messageBus = messageBus;
        _packetSerializer = packetSerializer;
        _readerWriterFactory = readerWriterFactory;
    }

    public async Task HandleNetworkPacket(int opCode, byte[] data)
    {
        var packetType = _packetMapping.GetPacketType(opCode);

        if (packetType != null)
        {
            using var stream = new MemoryStream(data);
            using var reader = _readerWriterFactory.CreateBinaryReader(stream);

            var packet = _packetSerializer.DeserializePacket(opCode, reader);

            var messageType = typeof(PacketReceivedMessage<>).MakeGenericType(packetType);
            var methodInfo = typeof(IPublishBus).GetMethod(nameof(IPublishBus.PublishAsync))?.MakeGenericMethod(messageType);

            await (Task?)methodInfo?.Invoke(_messageBus, new[] { Activator.CreateInstance(messageType, packet) })!;
            await _messageBus.PublishAsync(new RawPacketReceivedMessage(opCode, data));
        }
    }
}
