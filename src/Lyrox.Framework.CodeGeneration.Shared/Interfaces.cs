using System;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;

namespace Lyrox.Framework.CodeGeneration.Shared;

// Framework Logic

public interface IPacketParser<TPacket> where TPacket : class
{
    TPacket Deserialize(IMojangBinaryReader reader);
    void Serialize(IMojangBinaryWriter writer, TPacket packet);
}

[AttributeUsage(AttributeTargets.Class)]
public class AutoParsedAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class CustomParsedAttribute<TPacket, TParser> : Attribute
    where TPacket : class
    where TParser : IPacketParser<TPacket>
{ }

[AttributeUsage(AttributeTargets.Parameter)]
public class VarIntAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter)]
public class OptionalAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter)]
public class LengthPrefixedAttribute : Attribute { }
