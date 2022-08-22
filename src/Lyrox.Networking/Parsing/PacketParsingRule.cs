namespace Lyrox.Networking.Parsing
{
    public class PacketParsingRule
    {
        public int OPCode { get; }
        public Type MappedEventType { get; }
        public List<Mapping> Mappings { get; }

        public PacketParsingRule(int opCode, Type mappedEventType, List<Mapping> mappings)
        {
            OPCode = opCode;
            MappedEventType = mappedEventType;
            Mappings = mappings;
        }
    }
}
