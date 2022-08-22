namespace Lyrox.Networking.Parsing
{
    internal class PacketParsingConfiguration
    {
        private readonly List<PacketParsingRule> _packetParsingRules;

        public PacketParsingConfiguration()
            => _packetParsingRules = new();

        public PacketParsingConfiguration AddParsing(Action<PacketParsingRuleBuilder> configuration)
        {
            var builder = new PacketParsingRuleBuilder();
            configuration.Invoke(builder);
            var parsingRule = builder.Build();
            _packetParsingRules.Add(parsingRule);

            return this;
        }

        public PacketParsingRule? GetParsingRule(int opCode)
            => _packetParsingRules.FirstOrDefault(p => p.OPCode == opCode);
    }
}
