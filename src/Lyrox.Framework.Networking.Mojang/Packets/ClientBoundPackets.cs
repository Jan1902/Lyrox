using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Networking.Mojang.Packets.ClientBound;

[AutoSerializedPacket(0x00)]
public record DisconnectLogin(string Message) : IPacket;

[AutoSerializedPacket(0x20)]
public record KeepAliveCB(long KeepAliveID) : IPacket;

[AutoSerializedPacket(0x00)]
public record LoginSuccess(Guid UUID, string Username) : IPacket;

[AutoSerializedPacket(0x00)]
public record Test(string Message, bool[] FilledInventorySlot) : IPacket;
