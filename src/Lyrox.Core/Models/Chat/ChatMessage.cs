namespace Lyrox.Core.Models.Chat
{
    public class ChatMessage
    {
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; }

        public ChatMessage(DateTime timeStamp, string text)
        {
            TimeStamp = timeStamp;
            Text = text;
        }
    }
}
