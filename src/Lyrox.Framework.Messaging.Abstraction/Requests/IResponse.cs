namespace Lyrox.Framework.Base.Messaging.Abstraction.Requests;

public interface IResponse
{
    public bool Successful { get; }
    public IEnumerable<string>? Errors { get; }
}
