using Autofac;
using FluentAssertions;
using Lyrox.Framework.Base.Messaging.Abstraction;
using Lyrox.Framework.Base.Messaging.Abstraction.Core;
using Lyrox.Framework.Base.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Base.Messaging.Internal;
using Lyrox.Framework.Base.Messaging.Test.Mocks.Message;
using Lyrox.Framework.Base.Messaging.Test.Mocks.Request;
using Moq;

namespace Lyrox.Framework.Base.Messaging.Test.Tests;

public class MessageBusTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task MessageBus_Publish_ShouldCallHandler()
    {
        var handlerMock = new Mock<IMessageHandler<TestMessage>>();
        IMessageBus messageBus = new MessageBus();
        var message = new TestMessage();

        messageBus.Subscribe<TestMessage, IMessageHandler<TestMessage>>(handlerMock.Object);
        await messageBus.PublishAsync(message);

        handlerMock.Verify(h => h.HandleMessageAsync(message));
    }

    [Test]
    public async Task MessageBus_GetResponse_ShouldCallHandler()
    {
        var handlerMock = new Mock<IRequestHandler<TestRequest, TestResponse>>();
        IMessageBus messageBus = new MessageBus();
        var request = new TestRequest();

        messageBus.Subscribe<TestRequest, TestResponse, IRequestHandler<TestRequest, TestResponse>>(handlerMock.Object);
        var response = await messageBus.GetResponsesAsync<TestRequest, TestResponse>(request);

        handlerMock.Verify(h => h.HandleRequestAsync(request));
    }

    [Test]
    public void AutofacMessageBus_ShouldRegisterHandlers()
    {
        var builder = new ContainerBuilder();
        builder.RegisterAutofacMessagebus();

        var messageHandlerMock = new Mock<IMessageHandler<TestMessage>>();
        var requestHandlerMock = new Mock<IRequestHandler<TestRequest, TestResponse>>();

        builder.RegisterInstance(messageHandlerMock.Object).As<IMessageHandler<TestMessage>>().SingleInstance();
        builder.RegisterInstance(requestHandlerMock.Object).As<IRequestHandler<TestRequest, TestResponse>>().SingleInstance();

        var container = builder.Build();
        container.SetupAutofacMessageBus();
        var messageBus = container.Resolve<IMessageBus>();

        var messageBusInternal = messageBus as MessageBus;
        messageBus.Should().NotBeNull();

        messageBusInternal!.MessageHandlers.Should().ContainSingle().Which
            .Value.Should().ContainSingle().Which.Should().BeAssignableTo<IMessageHandler<TestMessage>>();
        messageBusInternal!.RequestHandlers.Should().ContainSingle().Which
            .Value.Should().ContainSingle().Which.Should().BeAssignableTo<IRequestHandler<TestRequest, TestResponse>>();
    }

    [Test]
    public void MessageBusSubscription_Dispose_ShouldUnsubscribe()
    {
        var handlerMock = new Mock<IMessageHandler<TestMessage>>();
        var messageBus = new MessageBus();

        var subscription = messageBus.Subscribe<TestMessage, IMessageHandler<TestMessage>>(handlerMock.Object);

        messageBus.MessageHandlers.Should().ContainSingle().Which.Value.Should().ContainSingle(h => h == handlerMock.Object);

        subscription.Dispose();

        messageBus.MessageHandlers.Should().NotContain(h => h.Value.Any());
    }

    [Test]
    public void AutofacMessageBus_Extensions_ShouldRegisterHandlers()
    {
        var builder = new ContainerBuilder();
        builder.RegisterMessageHandler<TestMessage, TestMessageHandler>();
        builder.RegisterRequestHandler<TestRequest, TestResponse, TestRequestHandler>();

        var container = builder.Build();
        container.ComponentRegistry.Registrations.Should().ContainSingle(h => h.Activator.LimitType == typeof(TestMessageHandler));
        container.ComponentRegistry.Registrations.Should().ContainSingle(h => h.Activator.LimitType == typeof(TestRequestHandler));
    }
}
