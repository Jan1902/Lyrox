using System.Security.Cryptography;
using System.Text;
using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.Chat.Mojang.Packets.ServerBound;

[CustomSerializedPacket<ChatCommand, ChatCommandSerializer>(0x04)]
internal record ChatCommand(string Command, params string[] Arguments) : IPacket;

internal class ChatCommandSerializer : IPacketSerializer<ChatCommand>
{
    public ChatCommand Deserialize(IMinecraftBinaryReader reader) => throw new NotImplementedException();
    public void Serialize(IMinecraftBinaryWriter writer, ChatCommand packet)
    {
        var salt = new Random().NextInt64();

        writer.WriteStringWithVarIntPrefix(packet.Command + " " + string.Join(" ", packet.Arguments));
        writer.WriteLong(DateTime.Now.Ticks);
        writer.WriteLong(salt);
        writer.WriteVarInt(packet.Arguments.Length);

        foreach (var argument in packet.Arguments)
        {
            var buffer = new byte[argument.Length + sizeof(long)];
            Array.Copy(Encoding.UTF8.GetBytes(argument), 0, buffer, 0, argument.Length);
            Array.Copy(System.BitConverter.GetBytes(salt), 0, buffer, 0, sizeof(long));
            var signature = SHA256.Create().ComputeHash(buffer);

            writer.WriteStringWithVarIntPrefix(argument);
            writer.WriteVarInt(signature.Length);
            writer.WriteBytes(signature);
        }

        writer.WriteBool(false);
    }
}
