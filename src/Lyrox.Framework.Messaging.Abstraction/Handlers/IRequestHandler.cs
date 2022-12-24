namespace Lyrox.Framework.Messaging.Abstraction.Handlers
{
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse
    {
        Task<TResponse> HandleRequestAsync(TRequest request);
    }
}
