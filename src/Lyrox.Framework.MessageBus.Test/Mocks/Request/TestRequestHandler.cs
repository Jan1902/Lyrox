using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Base.Messaging.Test.Mocks.Request;

internal class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
    public Task<TestResponse> HandleRequestAsync(TestRequest request)
        => Task.FromResult(new TestResponse());
}
