using FluentAssertions;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;
using Moq;

namespace Lyrox.Framework.CodeGeneration.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var reader = Mock.Of<IMojangBinaryReader>();

        var mapping = new PacketTypeMapping();
        mapping.AddMapping<KeepAliveCB>(0);

        var serializer = new PacketSerializer(mapping);

        var keepAliveCB = serializer.DeserializePacket(0, reader) as KeepAliveCB;

        keepAliveCB.Should().NotBeNull();
        keepAliveCB.KeepAliveID.Should().Be(0);
    }
}
