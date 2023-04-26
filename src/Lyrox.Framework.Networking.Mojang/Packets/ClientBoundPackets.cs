using Lyrox.Framework.CodeGeneration.Shared;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Framework.Networking.Mojang.Packets.ClientBound;

[AutoSerialized]
public record DisconnectLogin(string Message);

[AutoSerialized]
public record KeepAliveCB(long KeepAliveID) : IClientBoundNetworkPacket;

[AutoSerialized]
public record LoginSuccess(Guid UUID, string Username);
