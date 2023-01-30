using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Base.Messaging.Test.Mocks.Message;

internal class TestMessageHandler : IMessageHandler<TestMessage>
{
    public Task HandleMessageAsync(TestMessage message)
        => Task.CompletedTask;
}
