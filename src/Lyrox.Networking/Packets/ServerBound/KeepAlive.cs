namespace Lyrox.Networking.Packets.ServerBound
{
    internal class KeepAlive : ServerBoundPacket
    {
        public long KeepAliveID { get; }

        public override int OPCode => 0x11;

        public KeepAlive(long keepAliveID)
            => KeepAliveID = keepAliveID;

        public override byte[] Build()
        {
            AddLong(KeepAliveID);

            return GetBytes();
        }
    }
}
