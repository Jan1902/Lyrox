using Lyrox.Framework.Core.Events.Implementations;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Abstraction
{
    public interface ILyroxClient
    {
        Task Connect();
        event EventHandler<ChatMessageReceivedEvent> ChatMessageReceived;
        void SendChatMessage(string message);
        void SendPrivateMessage(string message, string player);
        void SendCommand(string command, string[] arguments);
        void Goto(Vector3d position);
        BlockState? GetBlock(Vector3i position);
        ChunkSection? GetChunkSection(Vector3i chunkPos);
    }
}
