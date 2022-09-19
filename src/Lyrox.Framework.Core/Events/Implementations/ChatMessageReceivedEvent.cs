namespace Lyrox.Framework.Core.Events.Implementations
{
    public class ChatMessageReceivedEvent : Event
    {
        public string Message { get; set; }
        public string Sender { get; set; }

        public ChatMessageReceivedEvent(string message, string sender)
        {
            Message = message;
            Sender = sender;
        }
    }
}
