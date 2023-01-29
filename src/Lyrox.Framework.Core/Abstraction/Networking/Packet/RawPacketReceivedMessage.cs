using Lyrox.Framework.Base.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Core.Abstraction.Networking.Packet.Handler;

public record RawPacketReceivedMessage : IMessage
{
    public int OpCode { get; }
    public byte[] Data { get; }

    public RawPacketReceivedMessage(int opCode, byte[] data)
    {
        OpCode = opCode;
        Data = data;
    }
}
