using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Networking.Types;

namespace Lyrox.Networking.Packets.ServerBound
{
    internal class LoginStart : ServerBoundPacket
    {
        public string Name { get; }
        public bool HasSigData { get; }
        public long TimeStamp { get; }
        public byte[] PublicKey { get; }
        public byte[] Signature { get; }

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

        public override byte[] Build()
        {
            AddVarInt((int)OPLogin.LoginStart);

            AddBool(HasSigData);

            if(!HasSigData)
                return GetBytes();

            AddLong(TimeStamp);
            AddVarInt(PublicKey.Length);
            AddBytes(PublicKey);
            AddVarInt(Signature.Length);
            AddBytes(Signature);

            return GetBytes();
        }
    }
}
