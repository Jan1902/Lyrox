using System.Security.Cryptography;
using System.Text;
using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Chat.Mojang.Packets.ServerBound;

[CustomSerializedPacket<ChatMessage, ChatMessageSerializer>(0x05)]
internal record ChatMessage(string Message) : IPacket;

internal class ChatMessageSerializer : IPacketSerializer<ChatMessage>
{
    public ChatMessage Deserialize(IMinecraftBinaryReader reader) => throw new NotImplementedException();
    public void Serialize(IMinecraftBinaryWriter writer, ChatMessage packet)
    {
        var salt = new Random().NextInt64();
        var buffer = new byte[packet.Message.Length + sizeof(long)];
        Array.Copy(Encoding.UTF8.GetBytes(packet.Message), 0, buffer, 0, packet.Message.Length);
        Array.Copy(System.BitConverter.GetBytes(salt), 0, buffer, 0, sizeof(long));
        var signature = SHA256.HashData(buffer);

        writer.WriteStringWithVarIntPrefix(packet.Message);
        writer.WriteLong(DateTime.Now.Ticks);
        writer.WriteLong(salt);
        writer.WriteVarInt(signature.Length);
        writer.WriteBytes(signature);
        writer.WriteBool(false);

        // Last seen messages
        writer.WriteVarInt(0);

        // Last message
        writer.WriteBool(false);
    }
}
