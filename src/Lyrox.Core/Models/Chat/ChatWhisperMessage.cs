namespace Lyrox.Core.Models.Chat
{
    public class ChatWhisperMessage : ChatMessage
    {
        public string FromPlayer { get; set; }
        public string ToPlayer { get; set; }

        public ChatWhisperMessage(DateTime timeStamp, string message, string fromPlayer, string toPlayer) : base(timeStamp, message)
        {
            FromPlayer = fromPlayer;
            ToPlayer = toPlayer;
        }
    }
}
