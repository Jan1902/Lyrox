using Lyrox.Framework.Messaging.Abstraction.Messages;

namespace Lyrox.Framework.Messaging.Abstraction.Core
{
    public interface IPublishBus
    {
        Task PublishAsync<TMessage>(TMessage message)
            where TMessage : IMessage;

        Task<TResponse[]> GetResponsesAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
            where TResponse : IResponse;
    }
}
