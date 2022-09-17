using Lyrox.Core.Models.Chat;

namespace Lyrox.Abstraction
{
    public interface ILyroxClient
    {
        Task Connect();
        event EventHandler<ChatMessage> ChatMessageReceived;
        void SendChatMessage(string message, string receiver = "");
    }
}
