using Lyrox.Framework.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Messaging.Test.Mocks.Message
{
    internal class TestMessageHandler : IMessageHandler<TestMessage>
    {
        public bool Called { get; private set; }

        public Task HandleMessageAsync(TestMessage message)
        {
            Called = true;
            return Task.CompletedTask;
        }
    }
}
