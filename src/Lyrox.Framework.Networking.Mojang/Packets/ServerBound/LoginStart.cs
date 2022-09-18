using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ServerBound
{
    internal class LoginStart : MojangServerBoundPacket
    {
        public string Name { get; }
        public bool HasSigData { get; }
        public long TimeStamp { get; }
        public byte[] PublicKey { get; }
        public byte[] Signature { get; }

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

            if (!HasSigData)
                return;

            Writer.WriteLong(TimeStamp);
            Writer.WriteVarInt(PublicKey.Length);
            Writer.WriteBytes(PublicKey);
            Writer.WriteVarInt(Signature.Length);
            Writer.WriteBytes(Signature);
        }
    }
}
