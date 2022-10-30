namespace Lyrox.Framework.Core.Events.Implementations
{
    public record ChatMessageReceivedEvent : EventBase
    {
        public string Message { get; init; }
        public string Sender { get; init; }

        public ChatMessageReceivedEvent(string message, string sender)
        {
            Message = message;
            Sender = sender;
        }
    }
}
