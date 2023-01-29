namespace Lyrox.Framework.Core.Abstraction.Managers;

public interface IChatManager
{
    Task SendChatMessage(string message);
    Task SendPrivateMessage(string message, string player);
    Task SendCommand(string command, string[] arguments);
}
