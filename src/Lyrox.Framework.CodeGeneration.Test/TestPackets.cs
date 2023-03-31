namespace Lyrox.Framework.CodeGeneration.Test
{
    [AutoParsed]
    public partial record Handshake : Packet
    {
        public int HandshakeID { get; private set; }
    }

    public record Login : Packet
    {
        public override void Parse()
        {

        }
    }

    public abstract record Packet
    {
        public abstract void Parse();
    }
}
