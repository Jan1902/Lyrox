using Autofac;
using FluentAssertions;
using Lyrox.Framework.Messaging.Abstraction.Core;
using Lyrox.Framework.Messaging.Abstraction.Handlers;
using Lyrox.Framework.Messaging.Internal;
using Lyrox.Framework.Messaging.Test.Mocks.Message;
using Lyrox.Framework.Messaging.Test.Mocks.Request;
using Moq;

namespace Lyrox.Framework.Messaging.Test.Tests
{
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
            handlerMock.Setup(m => m.HandleRequestAsync(It.IsAny<TestRequest>())).Returns(Task.FromResult(new TestResponse()));
            IMessageBus messageBus = new MessageBus();
            var request = new TestRequest();

            messageBus.Subscribe<TestRequest, TestResponse, IRequestHandler<TestRequest, TestResponse>>(handlerMock.Object);
            var response = await messageBus.GetResponsesAsync<TestRequest, TestResponse>(request);

            response.Should().ContainSingle().Which.Successful.Should().BeTrue();
            handlerMock.Verify(h => h.HandleRequestAsync(request));
        }

        [Test]
        public async Task AutofacMessageBus_ShouldRegisterHandlers()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAutofacMessagebus();
            builder.RegisterType<TestMessageHandler>().As<IMessageHandler<TestMessage>>().SingleInstance();

            var container = builder.Build();
            var messageBus = container.Resolve<IMessageBus>();

            await messageBus.PublishAsync(new TestMessage());

            ((TestMessageHandler)container.Resolve<IMessageHandler<TestMessage>>()).Called.Should().BeTrue();
        }

        [Test]
        public async Task MessageBusSubscription_Dispose_ShouldUnsubscribe()
        {
            var handlerMock = new Mock<IMessageHandler<TestMessage>>();
            IMessageBus messageBus = new MessageBus();
            var message = new TestMessage();

            var subscription = messageBus.Subscribe<TestMessage, IMessageHandler<TestMessage>>(handlerMock.Object);
            await messageBus.PublishAsync(message);

            handlerMock.Verify(h => h.HandleMessageAsync(message));

            subscription.Dispose();
            await messageBus.PublishAsync(message);

            handlerMock.Verify(h => h.HandleMessageAsync(message), Times.Once);
        }

        [Test]
        public void AutofacMessageBus_Extensions_ShouldRegisterHandlers()
        {
            var builder = new ContainerBuilder();
            builder.RegisterMessageHandler<TestMessage, TestMessageHandler>();

            var container = builder.Build();
            container.ComponentRegistry.Registrations.Should().ContainSingle(h => h.Activator.LimitType == typeof(TestMessageHandler));
        }
    }
}
