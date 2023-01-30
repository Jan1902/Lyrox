using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Messages;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Client.Abstraction;

public interface ILyroxClient
{
    Task Connect();
    event EventHandler<ChatMessageReceivedMessage> ChatMessageReceived;
    event EventHandler<ConnectionEstablishedMessage>? Connected;
    event EventHandler<ConnectionTerminatedMessage>? Disconnected;
    Task SendChatMessage(string message);
    Task SendPrivateMessage(string message, string player);
    Task SendCommand(string command, string[] arguments);
    void Goto(Vector3d position);
    BlockState? GetBlock(Vector3i position);
    ChunkSection? GetChunkSection(Vector3i chunkPos);
}
