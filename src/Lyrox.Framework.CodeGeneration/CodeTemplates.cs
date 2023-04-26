namespace Lyrox.Framework.CodeGeneration
{
    internal class CodeTemplates
    {
        public const string SerializerSkeleton = @"
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.CodeGeneration.Shared;

namespace {NAMESPACE}
{
    public class {PACKETNAME}_Serialization : IPacketSerializer<{PACKETNAME}>
    {
        public {PACKETNAME} Deserialize(IMojangBinaryReader reader)
        {
            {DESERIALIZECONTENT}
        }

        public void Serialize(IMojangBinaryWriter writer, {PACKETNAME} packet)
        {
			{SERIALIZECONTENT}
        }
    }
}";

        public const string SerializerMappingSkeleton = @"
{USINGS}

namespace Lyrox.Framework.Networking.Core
{
	public static class SerializerMappings
	{
		private static readonly Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>
		{
			{MAPPINGCONTENT}
		};

        public static bool TryGetSerializerType(Type packetType, out Type serializerType)
            => _mappings.TryGetValue(packetType, out serializerType);
	}
}";
    }
}
