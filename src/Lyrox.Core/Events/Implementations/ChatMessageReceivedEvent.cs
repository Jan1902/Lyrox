using Lyrox.Core.Models.Chat;

namespace Lyrox.Core.Events.Implementations
{
    internal class ChatMessageReceivedEvent : Event
    {
        public ChatMessage ChatMessage { get; set; }

        public ChatMessageReceivedEvent(ChatMessage chatMessage)
            => ChatMessage = chatMessage;
    }
}
