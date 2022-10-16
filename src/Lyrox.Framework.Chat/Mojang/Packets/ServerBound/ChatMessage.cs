using System.Security.Cryptography;
using System.Text;
using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Chat.Mojang.Packets.ServerBound
{
    internal class ChatMessage : MojangServerBoundPacket
    {
        public string Message { get; }

        public override int OPCode => 0x05;

        public ChatMessage(string message)
            => Message = message;

        public override void Build()
        {
            var salt = new Random().NextInt64();
            var buffer = new byte[Message.Length + sizeof(long)];
            Array.Copy(Encoding.UTF8.GetBytes(Message), 0, buffer, 0, Message.Length);
            Array.Copy(System.BitConverter.GetBytes(salt), 0, buffer, 0, sizeof(long));
            var signature = SHA256.Create().ComputeHash(buffer);

            Writer.WriteStringWithVarIntPrefix(Message);
            Writer.WriteLong(DateTime.Now.Ticks);
            Writer.WriteLong(salt);
            Writer.WriteVarInt(signature.Length);
            Writer.WriteBytes(signature);
            Writer.WriteBool(false);

            // Last seen messages
            Writer.WriteVarInt(0);

            // Last message
            Writer.WriteBool(false);
        }
    }
}
