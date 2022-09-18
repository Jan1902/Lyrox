using System.Collections.Immutable;

namespace Lyrox.Framework.Core.Modules.Abstractions
{
    public interface IServiceContainer
    {
        ImmutableDictionary<Type, object> GetInstanceServices();
        ImmutableDictionary<Type, Type> GetTypeServices();
        void RegisterInstance<T>(object instance);
        void RegisterType<TAs, TType>() where TType : TAs;
    }
}
