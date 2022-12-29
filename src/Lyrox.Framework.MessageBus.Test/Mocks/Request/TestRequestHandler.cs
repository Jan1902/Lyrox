using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lyrox.Framework.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Messaging.Test.Mocks.Request
{
    internal class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<TestResponse> HandleRequestAsync(TestRequest request)
            => Task.FromResult(new TestResponse());
    }
}
