using Lyrox.Core.Events;
using Lyrox.Core.Models.Chat;

namespace Lyrox.Chat.Events
{
    public class ChatMessageReceivedEvent : Event
    {
        public ChatMessage Message { get; }

        public ChatMessageReceivedEvent(object sender, ChatMessage message)
        {
            Message = message;
        }
    }
}
