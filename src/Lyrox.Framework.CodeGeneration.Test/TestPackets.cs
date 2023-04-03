namespace Lyrox.Framework.CodeGeneration.Test
{
    [AutoParsed]
    public partial record Handshake : Packet
    {
        public int HandshakeID { get; private set; }
    }

    public record Login : Packet
    {
        public string Name { get; private set; }

        public override void Parse()
        {
            var length = Reader.ReadInt();
            Name = Reader.ReadString(length);
        }
    }

    public abstract record Packet
    {
        protected Reader Reader = new Reader();

        public abstract void Parse();
    }

    public class Reader
    {
        public int ReadInt() => 1;
        public string ReadString(int length) => "Hello World";
    }
}
