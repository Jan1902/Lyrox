namespace Lyrox.Framework.Base.Messaging.Abstraction.Exceptions;

public class MessagingException : Exception
{
    public MessagingException(string? message) : base(message)
    {
    }
}
