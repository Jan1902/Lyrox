using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Chat.Mojang.Packets.ClientBound
{
    public class PlayerChatMessage : MojangClientBoundPacket
    {
        public string JSONSigned { get; private set; }
        public string JSONUnsigned { get; private set; }
        public ChatMessageType MessageType { get; private set; }
        public Guid SenderUUID { get; private set; }
        public string SenderNameJSON { get; private set; }
        public string SenderTeamNameJSOn { get; private set; }
        public DateTime Timestamp { get; private set; }
        public long Salt { get; private set; }
        public byte[] Signature { get; private set; }

        public override void Parse()
        {
            JSONSigned = Reader.ReadStringWithVarIntPrefix();
            if (Reader.ReadBool())
                JSONUnsigned = Reader.ReadStringWithVarIntPrefix();

            MessageType = (ChatMessageType)Reader.ReadVarInt();
            SenderUUID = Reader.ReadUUID();
            SenderNameJSON = Reader.ReadStringWithVarIntPrefix();
            if (Reader.ReadBool())
                SenderTeamNameJSOn = Reader.ReadStringWithVarIntPrefix();

            Timestamp = new DateTime(Reader.ReadLong());
            Salt = Reader.ReadLong();
            Signature = Reader.ReadBytes(Reader.ReadVarInt());
        }
    }
}
