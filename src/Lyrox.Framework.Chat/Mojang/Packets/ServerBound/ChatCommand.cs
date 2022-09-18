using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Chat.Mojang.Packets.ServerBound
{
    internal class ChatCommand : MojangServerBoundPacket
    {
        public string Command { get; }
        public string[] Arguments { get; }
        public override int OPCode => 0x03;

        public ChatCommand(string command, params string[] arguments)
        {
            Command = command;
            Arguments = arguments;
        }

        public override void Build()
        {
            var salt = new Random().NextInt64();

            Writer.WriteStringWithVarIntPrefix(Command + " " + string.Join(" ", Arguments));
            Writer.WriteLong(DateTime.Now.Ticks);
            Writer.WriteLong(salt);
            Writer.WriteVarInt(Arguments.Length);

            foreach (var argument in Arguments)
            {
                var buffer = new byte[argument.Length + sizeof(long)];
                Array.Copy(Encoding.UTF8.GetBytes(argument), 0, buffer, 0, argument.Length);
                Array.Copy(System.BitConverter.GetBytes(salt), 0, buffer, 0, sizeof(long));
                var signature = SHA256.Create().ComputeHash(buffer);

                Writer.WriteStringWithVarIntPrefix(argument);
                Writer.WriteVarInt(signature.Length);
                Writer.WriteBytes(signature);
            }
            
            Writer.WriteBool(false);
        }
    }
}
