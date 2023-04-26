using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using static Lyrox.Framework.Chat.Mojang.Packets.ClientBound.PlayerChatMessage;

namespace Lyrox.Framework.Chat.Mojang.Packets.ClientBound;

[CustomSerialized<PlayerChatMessage, PlayerChatMessageSerializer>]
internal record PlayerChatMessage : IClientBoundNetworkPacket
{
    public byte[]? MessageSignature { get; }
    public Guid SenderUUID { get; }
    public byte[] HeaderSignature { get; }
    public string PlainMessage { get; }
    public string? FormattedMessage { get; }
    public DateTime Timestamp { get; }
    public long Salt { get; }
    public string? UnsignedContent { get; }
    public FilterType Filter { get; }
    public int ChatType { get; }
    public string NetworkName { get; }
    public string? NetworkTargetName { get; }

    public PlayerChatMessage(
        byte[]? messageSignature,
        Guid senderUUID,
        byte[] headerSignature,
        string plainMessage,
        string? formattedMessage,
        DateTime timestamp,
        long salt,
        string? unsignedContent,
        FilterType filter,
        int chatType,
        string networkName,
        string? networkTargetName)
    {
        MessageSignature = messageSignature;
        SenderUUID = senderUUID;
        HeaderSignature = headerSignature;
        PlainMessage = plainMessage;
        FormattedMessage = formattedMessage;
        Timestamp = timestamp;
        Salt = salt;
        UnsignedContent = unsignedContent;
        Filter = filter;
        ChatType = chatType;
        NetworkName = networkName;
        NetworkTargetName = networkTargetName;
    }

    internal enum FilterType
    {
        PASS_THROUGH = 0,
        FULLY_FILTERED = 1,
        PARTIALLY_FILTERED = 2
    }
}

internal class PlayerChatMessageSerializer : IPacketSerializer<PlayerChatMessage>
{
    public PlayerChatMessage Deserialize(IMojangBinaryReader reader)
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

    public void Serialize(IMojangBinaryWriter writer, PlayerChatMessage packet)
        => throw new NotImplementedException();
}
