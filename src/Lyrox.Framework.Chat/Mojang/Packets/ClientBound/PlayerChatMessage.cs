using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Chat.Mojang.Packets.ClientBound
{
    internal class PlayerChatMessage : MojangClientBoundPacket
    {
        public byte[] MessageSignature { get; private set; }
        public Guid SenderUUID { get; private set; }
        public byte[] HeaderSignature { get; private set; }
        public string PlainMessage { get; private set; }
        public string FormattedMessage { get; private set; }
        public DateTime Timestamp { get; private set; }
        public long Salt { get; private set; }
        public string UnsignedContent { get; private set; }
        public FilterType Filter { get; private set; }
        public int ChatType { get; private set; }
        public string NetworkName { get; private set; }
        public string NetworkTargetName { get; private set; }

        public override void Parse()
        {
            if (Reader.ReadBool())
                MessageSignature = Reader.ReadBytes(Reader.ReadVarInt());

            SenderUUID = Reader.ReadUUID();
            HeaderSignature = Reader.ReadBytes(Reader.ReadVarInt());
            PlainMessage = Reader.ReadStringWithVarIntPrefix();
            if (Reader.ReadBool())
                FormattedMessage = Reader.ReadStringWithVarIntPrefix();
            Timestamp = new DateTime(Reader.ReadLong());
            Salt = Reader.ReadLong();

            // Skip previous message information
            for (var i = 0; i < Reader.ReadVarInt(); i++)
            {
                Reader.ReadUUID();
                Reader.ReadBytes(Reader.ReadVarInt());
            }

            if (Reader.ReadBool())
                UnsignedContent = Reader.ReadStringWithVarIntPrefix();

            Filter = (FilterType)Reader.ReadVarInt();
            if (Filter == FilterType.PARTIALLY_FILTERED)
                Reader.ReadBytes(Reader.ReadVarInt());

            ChatType = Reader.ReadVarInt();
            NetworkName = Reader.ReadStringWithVarIntPrefix();
            if(Reader.ReadBool())
                NetworkTargetName = Reader.ReadStringWithVarIntPrefix();
        }

        internal enum FilterType
        {
            PASS_THROUGH = 0,
            FULLY_FILTERED = 1,
            PARTIALLY_FILTERED = 2
        }
    }
}
