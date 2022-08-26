namespace Lyrox.Networking.Packets.ClientBound
{
    internal class DisconnectLogin : ClientBoundPacket
    {
        public string Message { get; private set; }

        public override void Parse()
        {
            Message = GetString();
        }
    }
}
