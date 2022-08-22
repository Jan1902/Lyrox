using Lyrox.Core.Events;

namespace Lyrox.Networking.Parsing
{
    public class NetworkPacketParser : INetworkPacketParser
    {
        private PacketParsingConfiguration? _parsingConfiguration;

        public NetworkPacketParser()
            => Configure();

        public Event? ParsePacket(int opCode, byte[] data)
        {
            var parsingRule = _parsingConfiguration?.GetParsingRule(opCode);
            if (parsingRule == null)
                return null;

            var evt = Activator.CreateInstance(parsingRule.MappedEventType) as Event;

            foreach (var mapping in parsingRule.Mappings)
            {
                //TODO: Implement this
            }

            return evt;
        }

        private void Configure()
        {
            _parsingConfiguration = new PacketParsingConfiguration();

            _parsingConfiguration
                .AddParsing(p =>
                {
                    p.ForOpCode(0x00)
                    .MapToEvent<Event>()
                    .MapLengthPrefixedString("Text");
                })
                .AddParsing(p =>
                {
                    p.ForOpCode(0x01)
                    .MapToEvent<Event>()
                    .MapByte("Data")
                    .AsLengthPrefixedArray();
                });

            Console.WriteLine();
        }
    }
}
