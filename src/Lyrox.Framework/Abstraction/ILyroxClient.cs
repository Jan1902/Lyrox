using Lyrox.Framework.Core.Events.Implementations;

namespace Lyrox.Framework.Abstraction
{
    public interface ILyroxClient
    {
        Task Connect();
        event EventHandler<ChatMessageReceivedEvent> ChatMessageReceived;
        void SendChatMessage(string message);
        void SendPrivateMessage(string message, string player);
        void SendCommand(string command, string[] arguments);
    }
}
