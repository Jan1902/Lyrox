using Lyrox.Framework.Base.Messaging.Abstraction.Messages;
using Lyrox.Framework.Base.Messaging.Abstraction.Requests;

namespace Lyrox.Framework.Base.Messaging.Abstraction.Core;

public interface IPublishBus
{
    Task PublishAsync<TMessage>(TMessage message)
        where TMessage : IMessage;

    Task<TResponse[]> GetResponsesAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse;
}
