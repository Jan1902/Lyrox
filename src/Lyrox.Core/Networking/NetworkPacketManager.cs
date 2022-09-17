using Autofac;
using Lyrox.Core.Networking.Abstraction;
using Lyrox.Core.Networking.Abstraction.Handler;
using Lyrox.Core.Networking.Abstraction.Packet;

namespace Lyrox.Core.Networking
{
    public class NetworkPacketManager : INetworkPacketManager
    {
        private readonly Dictionary<Type, List<object>> _handlers;
        private readonly Dictionary<int, List<object>> _rawHandlers;

        private readonly ContainerBuilder _containerBuilder;

        private readonly Dictionary<int, (Type PacketType, List<Type> Handlers)> _handlerMapping;
        private readonly Dictionary<int, List<Type>> _rawHandlerMapping;

        public NetworkPacketManager(ContainerBuilder containerBuilder)
        {
            _handlers = new();
            _handlerMapping = new();
            _rawHandlers = new();
            _rawHandlerMapping = new();

            _containerBuilder = containerBuilder;
        }

        public void HandleNetworkPacket(int opCode, byte[] data)
        {
            if (_handlerMapping.ContainsKey(opCode))
            {
                var (packetType, _) = _handlerMapping[opCode];
                var packet = Activator.CreateInstance(packetType) as IClientBoundNetworkPacket;
                packet?.ParsePacket(data);

                var handlers = _handlers[packetType];

                foreach (var handler in handlers)
                {
                    typeof(IPacketHandler<>)
                        .MakeGenericType(packetType)
                        .GetMethods()[0]
                        .Invoke(handler, new object[] { packet });
                }
            }

            if (_rawHandlerMapping.ContainsKey(opCode))
            {
                foreach (var rawHandler in _rawHandlers[opCode].Select(h => h as IRawPacketHandler))
                    rawHandler?.HandlePacket(opCode, data);
            }
        }

        public void RegisterNetworkPacketHandler<TPacket, THandler>(int opCode)
            where TPacket : IClientBoundNetworkPacket
            where THandler : IPacketHandler<TPacket>
        {
            if (!_handlerMapping.Values.Any(l => l.Handlers.Contains(typeof(THandler))))
                _containerBuilder.RegisterType<THandler>().InstancePerLifetimeScope();

            if (!_handlerMapping.ContainsKey(opCode))
                _handlerMapping[opCode] = new(typeof(TPacket), new());

            _handlerMapping[opCode].Handlers.Add(typeof(THandler));
        }

        public void RegisterPacketHandlersFromContainer(IContainer container)
        {
            foreach (var opCode in _handlerMapping.Keys)
            {
                foreach (var handlerType in _handlerMapping[opCode].Handlers)
                {
                    var handler = container.Resolve(handlerType);
                    RegisterNetworkPacketHandler(_handlerMapping[opCode].PacketType, handler);
                }
            }

            foreach (var opCode in _rawHandlerMapping.Keys)
            {
                foreach (var handlerType in _rawHandlerMapping[opCode])
                {
                    var handler = container.Resolve(handlerType);
                    RegisterRawNetworkPacketHandler(opCode, handler);
                }
            }
        }

        public void RegisterNetworkPacketHandler(Type packetType, object handler)
        {
            if (packetType == null)
                throw new ArgumentNullException(nameof(packetType));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlers = new List<object>();

            if (_handlers.ContainsKey(packetType))
                handlers = _handlers[packetType];
            else
                _handlers[packetType] = handlers;

            handlers.Add(handler);
        }

        public void RegisterRawNetworkPacketHandler<THandler>(int opCode)
            where THandler : IRawPacketHandler
        {
            if (!_rawHandlerMapping.Values.Any(l => l.Contains(typeof(THandler))))
                _containerBuilder.RegisterType<THandler>().InstancePerLifetimeScope();

            if (!_rawHandlerMapping.ContainsKey(opCode))
                _rawHandlerMapping[opCode] = new();

            _rawHandlerMapping[opCode].Add(typeof(THandler));
        }

        public void RegisterRawNetworkPacketHandler(int opCode, object handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var handlers = new List<object>();

            if (_rawHandlers.ContainsKey(opCode))
                handlers = _rawHandlers[opCode];
            else
                _rawHandlers[opCode] = handlers;

            handlers.Add(handler);
        }
    }
}
