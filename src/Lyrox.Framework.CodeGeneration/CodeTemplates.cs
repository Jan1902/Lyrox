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
using System.Collections.Immutable;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Lyrox.Framework.CodeGeneration.Shared;

namespace {NAMESPACE}
{
	public static class SerializerMappings
	{
		public static readonly ImmutableDictionary<Type, Type> _mappings = new Dictionary<Type, Type>
		{
			{MAPPINGCONTENT}
		}.ToImmutableDictionary();
	}
}";
    }
}
