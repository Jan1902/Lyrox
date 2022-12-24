using Lyrox.Framework.Messaging.Abstraction;
using Lyrox.Framework.Messaging.Abstraction.Handlers;

namespace Lyrox.Framework.Messaging.Internal
{
    internal class RequestHandleWrapper<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        private readonly Func<TRequest, Task<TResponse>> _handleFunction;

        public RequestHandleWrapper(Func<TRequest, Task<TResponse>> handleFunction)
            => _handleFunction = handleFunction;

        public Task<TResponse> HandleRequestAsync(TRequest request)
            => _handleFunction(request);
    }
}
