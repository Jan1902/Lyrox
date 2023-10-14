using System.Collections.Concurrent;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet;

public class PacketTypeMapping
{
    private readonly ConcurrentDictionary<int, Type> _packetTypeMapping;

    public PacketTypeMapping()
        => _packetTypeMapping = new();

    public void AddMapping<TPacket>(int opCode) where TPacket : IPacket
        => _packetTypeMapping.TryAdd(opCode, typeof(TPacket));

    public Type? GetPacketType(int opCode)
        => _packetTypeMapping.TryGetValue(opCode, out var type) ? type : default;

    public IEnumerable<(int ID, Type PacketType)> GetAllPacketMappings()
        => _packetTypeMapping.ToList().Select(m => (m.Key, m.Value));
}
