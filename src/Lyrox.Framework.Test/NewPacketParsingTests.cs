namespace Lyrox.Framework.Test;

public class NewPacketParsingTests
{
    //private PacketSerializer _packetSerializer = new PacketSerializer();

    //[Test]
    //public void Test_PrimitiveTypes()
    //{
    //    using var stream = new MemoryStream();
    //    using var writer = new MojangBinaryWriter(stream);

    //    var id = 1234;

    //    writer.WriteLong(id);
    //    stream.Seek(0, SeekOrigin.Begin);

    //    using var reader = new MojangBinaryReader(stream);
    //    var packet = _packetSerializer.DeserializePacket(typeof(KeepAliveCB), reader) as KeepAliveCB;

    //    packet.Should().NotBeNull();
    //    packet.KeepAliveID.Should().Be(id);
    //}

    //[Test]
    //public void Test_CustomTypes()
    //{
    //    using var stream = new MemoryStream();
    //    using var writer = new MojangBinaryWriter(stream);

    //    var position = new Vector3i(1, 2, 3);
    //    var id = 123;

    //    writer.WritePosition(position);
    //    writer.WriteVarInt(id);
    //    stream.Seek(0, SeekOrigin.Begin);

    //    using var reader = new MojangBinaryReader(stream);
    //    var packet = _packetSerializer.DeserializePacket(typeof(BlockUpdate), reader) as BlockUpdate;

    //    packet.Should().NotBeNull();
    //    packet.Location.Should().Be(position);
    //    packet.ID.Should().Be(id);
    //}

    //[Test]
    //public void Test_Complex()
    //{
    //    using var stream = new MemoryStream();
    //    using var writer = new MojangBinaryWriter(stream);

    //    var messageSignature = new byte[] { 1, 2, 3, 4, 5 };
    //    var senderUuid = Guid.NewGuid();
    //    var headerSignature = new byte[] { 4, 3, 2, 1 };
    //    var plainMessage = "hello";
    //    var timestamp = DateTime.Now;
    //    var salt = 42;

    //    writer.WriteBool(true);
    //    writer.WriteVarInt(messageSignature.Length);
    //    writer.WriteBytes(messageSignature);
    //    writer.WriteUUID(senderUuid);
    //    writer.WriteVarInt(headerSignature.Length);
    //    writer.WriteBytes(headerSignature);
    //    writer.WriteStringWithVarIntPrefix(plainMessage);
    //    writer.WriteBool(false);
    //    writer.WriteLong(timestamp.Ticks);
    //    writer.WriteLong(salt);

    //    stream.Seek(0, SeekOrigin.Begin);

    //    using var reader = new MojangBinaryReader(stream);
    //    var packet = _packetSerializer.DeserializePacket(typeof(PlayerChatMessage), reader) as PlayerChatMessage;

    //    packet.Should().NotBeNull();
    //    packet.MessageSignature.Should().BeEquivalentTo(messageSignature);
    //    packet.SenderUUID.Should().Be(senderUuid);
    //    packet.HeaderSignature.Should().BeEquivalentTo(headerSignature);
    //    packet.PlainMessage.Should().Be(plainMessage);
    //    packet.Timestamp.Should().Be(timestamp);
    //    packet.Salt.Should().Be(salt);
    //}
}
