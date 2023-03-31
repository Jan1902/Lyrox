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
        Packet packet = new Login();
        packet.Parse();

        packet = new Handshake();
        packet.Parse();
    }
}
