using Lyrox.Core.Events;

namespace Lyrox.Networking.Events
{
    internal class LoginSucessfulEvent : Event
    {
        public Guid UUID { get; }
        public string Username { get; }

        public LoginSucessfulEvent(Guid uUID, string username)
        {
            UUID = uUID;
            Username = username;
        }
    }
}
