namespace Lyrox.Framework.Base.Messaging.Abstraction.Core;

public class MessageBusSubscription : IDisposable
{
    private readonly Action _disposeAction;

    public MessageBusSubscription(Action disposeAction)
        => _disposeAction = disposeAction;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    public void Dispose()
        => _disposeAction();
}
