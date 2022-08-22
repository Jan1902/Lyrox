using Lyrox.Networking.Types;

namespace Lyrox.Networking.Parsing
{
    public class Mapping
    {
        public ProtocolDataTypes Type { get; set; }
        public string Field { get; set; }
        public bool IsArray { get; set; } = false;
        public int ArrayLength { get; set; } = 0;

        public Mapping(ProtocolDataTypes type, string field)
        {
            Type = type;
            Field = field;
        }

        public Mapping(ProtocolDataTypes type, string field, bool isArray)
        {
            Type = type;
            Field = field;
            IsArray = isArray;
        }
    }
}
