using FluentAssertions;
using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Networking;
using Lyrox.Framework.Networking.Mojang.Data.Abstraction;
using Moq;

namespace Lyrox.Framework.CodeGeneration.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var reader = Mock.Of<IMojangBinaryReader>();

        var handshake = new Handshake_Parsing().Deserialize(reader);
        var test = new Test_Parsing().Deserialize(reader);

        var parserType = SerializerMappings.Mappings[typeof(Login)];
        var parser = Activator.CreateInstance(parserType) as IPacketSerializer<Login>;

        var login = parser.Deserialize(reader);

        handshake.Should().NotBeNull();
        test.Should().NotBeNull();
    }
}
