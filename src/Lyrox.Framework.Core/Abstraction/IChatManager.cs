namespace Lyrox.Framework.Core.Abstraction
{
    public interface IChatManager
    {
        void SendChatMessage(string message);
        void SendPrivateMessage(string message, string player);
        void SendCommand(string command, string[] arguments);
    }
}
