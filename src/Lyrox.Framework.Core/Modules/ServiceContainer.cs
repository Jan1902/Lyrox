using System.Collections.Immutable;
using Lyrox.Framework.Core.Modules.Abstractions;

namespace Lyrox.Framework.Core.Modules
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly Dictionary<Type, Type> _typeServices;
        private readonly Dictionary<Type, object> _instanceServices;

        public ServiceContainer()
        {
            _typeServices = new();
            _instanceServices = new();
        }

        public void RegisterInstance<T>(object instance)
            => _instanceServices[typeof(T)] = instance;

        public void RegisterType<TAs, TType>() where TType : TAs
            => _typeServices.Add(typeof(TAs), typeof(TType));

        public ImmutableDictionary<Type, Type> GetTypeServices()
            => _typeServices.ToImmutableDictionary();

        public ImmutableDictionary<Type, object> GetInstanceServices()
            => _instanceServices.ToImmutableDictionary();
    }
}
