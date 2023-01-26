using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

namespace Lyrox.Framework.Core.Abstraction.Networking;

public class NetworkPacketManager : INetworkPacketManager
{
    private readonly PacketTypeMapping _packetMapping;
    private readonly IMessageBus _messageBus;

    public NetworkPacketManager(PacketTypeMapping packetMapping, IMessageBus messageBus)
    {
        _packetMapping = packetMapping;
        _messageBus = messageBus;
    }

    public async Task HandleNetworkPacket(int opCode, byte[] data)
    {
        var packetType = _packetMapping.GetPacketType(opCode);
        if (packetType != null)
        {
            var packet = Activator.CreateInstance(packetType) as IClientBoundNetworkPacket;
            packet?.ParsePacket(data);

            var messageType = typeof(PacketReceivedMessage<>).MakeGenericType(packetType);
            var methodInfo = typeof(IPublishBus).GetMethod(nameof(IPublishBus.PublishAsync))?.MakeGenericMethod(messageType);

            await (Task?)methodInfo?.Invoke(_messageBus, new[] { Activator.CreateInstance(messageType, packet) })!;
            await _messageBus.PublishAsync(new RawPacketReceivedMessage(opCode, data));
        }
    }
}
