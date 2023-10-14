using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using static Lyrox.Framework.Chat.Mojang.Packets.ClientBound.PlayerChatMessage;

namespace Lyrox.Framework.Chat.Mojang.Packets.ClientBound;

[CustomSerializedPacket<PlayerChatMessage, PlayerChatMessageSerializer>(0x33)]
internal record PlayerChatMessage(byte[]? MessageSignature, Guid SenderUUID, byte[] HeaderSignature, string PlainMessage, string? FormattedMessage, DateTime TimeStamp, long Salt, string? UnsignedContent, FilterType Filter, int ChatType, string NetworkName, string? NetworkTargetName) : IPacket
{
    internal enum FilterType
    {
        PASS_THROUGH = 0,
        FULLY_FILTERED = 1,
        PARTIALLY_FILTERED = 2
    }
}

internal class PlayerChatMessageSerializer : IPacketSerializer<PlayerChatMessage>
{
    public PlayerChatMessage Deserialize(IMinecraftBinaryReader reader)
    {
        byte[]? messageSignature = null;
        if (reader.ReadBool())
            messageSignature = reader.ReadBytes(reader.ReadVarInt());

        var senderUUID = reader.ReadUUID();
        var headerSignature = reader.ReadBytes(reader.ReadVarInt());
        var plainMessage = reader.ReadStringWithVarIntPrefix();

        string? formattedMessage = null;
        if (reader.ReadBool())
            formattedMessage = reader.ReadStringWithVarIntPrefix();

        var timestamp = new DateTime(reader.ReadLong());
        var salt = reader.ReadLong();

        // Skip previous message information
        for (var i = 0; i < reader.ReadVarInt(); i++)
        {
            reader.ReadUUID();
            reader.ReadBytes(reader.ReadVarInt());
        }

        string? unsignedContent = null;
        if (reader.ReadBool())
            unsignedContent = reader.ReadStringWithVarIntPrefix();

        var filter = (FilterType)reader.ReadVarInt();
        if (filter == FilterType.PARTIALLY_FILTERED)
            reader.ReadBytes(reader.ReadVarInt());

        var chatType = reader.ReadVarInt();
        var networkName = reader.ReadStringWithVarIntPrefix();

        string? networkTargetName = null;
        if (reader.ReadBool())
            networkTargetName = reader.ReadStringWithVarIntPrefix();

        return new PlayerChatMessage(
            messageSignature,
            senderUUID,
            headerSignature,
            plainMessage,
            formattedMessage,
            timestamp,
            salt,
            unsignedContent,
            filter,
            chatType,
            networkName,
            networkTargetName);
    }

    public void Serialize(IMinecraftBinaryWriter writer, PlayerChatMessage packet)
        => throw new NotImplementedException();
}
