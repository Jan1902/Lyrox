using Lyrox.Core.Events;
using Lyrox.Networking.Types;

namespace Lyrox.Networking.Parsing
{
    internal class PacketParsingRuleBuilder
    {
        private int? _opCode;
        private Type? _mappedEventType;
        private readonly List<Mapping> _mappings;

        private Mapping? _currentMapping = null;

        private bool _isDone;

        private bool IsValid => _opCode != null && _mappedEventType != null;

        public PacketParsingRuleBuilder()
            => _mappings = new();

        private void CheckAndFinishMapping()
        {
            if (_isDone)
                throw new Exception("Can't configure an already built Parsing Rule");

            if(_currentMapping != null)
                _mappings.Add(_currentMapping);
            _currentMapping = null;
        }

        public PacketParsingRuleBuilder ForOpCode(int opCode)
        {
            CheckAndFinishMapping();
            _opCode = opCode;
            return this;
        }

        public PacketParsingRuleBuilder MapToEvent<T>() where T : Event
        {
            CheckAndFinishMapping();
            _mappedEventType = typeof(T);
            return this;
        }

        public PacketParsingRuleBuilder MapToEvent(Type eventType)
        {
            CheckAndFinishMapping();
            _mappedEventType = eventType;
            return this;
        }

        public PacketParsingRuleBuilder MapLengthPrefixedString(string field)
        {
            CheckAndFinishMapping();

            _currentMapping = new Mapping(ProtocolDataTypes.String, field);
            return this;
        }

        public PacketParsingRuleBuilder MapVarInt(string field)
        {
            CheckAndFinishMapping();

            _currentMapping = new Mapping(ProtocolDataTypes.VarInt, field);
            return this;
        }

        public PacketParsingRuleBuilder MapByte(string field)
        {
            CheckAndFinishMapping();

            _currentMapping = new Mapping(ProtocolDataTypes.Byte, field);
            return this;
        }

        public PacketParsingRuleBuilder AsLengthPrefixedArray()
        {
            if (_currentMapping == null)
                throw new Exception("Invalid Packet Parsing Rule specified!");

            _currentMapping.IsArray = true;

            CheckAndFinishMapping();
            return this;
        }

        public PacketParsingRuleBuilder AsArrayWithLength(int length)
        {
            if(_currentMapping == null)
                throw new Exception("Invalid Packet Parsing Rule specified!");

            _currentMapping.IsArray = true;
            _currentMapping.ArrayLength = length;

            CheckAndFinishMapping();
            return this;
        }

        public PacketParsingRule Build()
        {
            CheckAndFinishMapping();

            if (!IsValid)
                throw new Exception("Invalid Packet Parsing Rule specified!");

            _isDone = true;

            return new PacketParsingRule(_opCode ?? 0, _mappedEventType, _mappings);
        }
    }
}
