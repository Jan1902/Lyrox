using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ServerBound
{
    internal class LoginStart : MojangServerBoundPacketBase
    {
        public string Name { get; init; }
        public bool HasSigData { get; init; }
        public long TimeStamp { get; init; }
        public byte[] PublicKey { get; init; }
        public byte[] Signature { get; init; }

        public override int OPCode => 0x00;

        public LoginStart(string name)
        {
            Name = name;
            HasSigData = false;
        }

        public LoginStart(string name, bool hasSigData, long timeStamp, byte[] publicKey, byte[] signature)
        {
            Name = name;
            HasSigData = hasSigData;
            TimeStamp = timeStamp;
            PublicKey = publicKey;
            Signature = signature;
        }

        public override void Build()
        {
            Writer.WriteStringWithVarIntPrefix(Name);
            Writer.WriteBool(HasSigData);

            if (HasSigData)
            {
                Writer.WriteLong(TimeStamp);
                Writer.WriteVarInt(PublicKey.Length);
                Writer.WriteBytes(PublicKey);
                Writer.WriteVarInt(Signature.Length);
                Writer.WriteBytes(Signature);
            }

            // Player UUID
            Writer.WriteBool(false);
        }
    }
}
