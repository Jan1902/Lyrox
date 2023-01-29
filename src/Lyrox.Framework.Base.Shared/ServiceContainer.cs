using System.Collections.Immutable;

namespace Lyrox.Framework.Base.Shared;

public class ServiceContainer
{
    private readonly List<(Type As, Type Type)> _typeServices;
    private readonly List<(Type As, object Instance)> _instanceServices;

    public ServiceContainer()
    {
        _typeServices = new();
        _instanceServices = new();
    }

    public void RegisterInstance<T>(object instance)
    {
        if (!_instanceServices.Any(s => s.As == typeof(T) && s.Instance == instance))
            _instanceServices.Add((typeof(T), instance));
    }

    public void RegisterType<TAs, TType>() where TType : TAs
    {
        if (!_typeServices.Any(s => s.As == typeof(TAs) && s.Type == typeof(TType)))
            _typeServices.Add((typeof(TAs), typeof(TType)));
    }

    public void RegisterType<TType>()
    {
        if (!_typeServices.Any(s => s.As == typeof(TType) && s.Type == typeof(TType)))
            _typeServices.Add((typeof(TType), typeof(TType)));
    }

    public IEnumerable<(Type As, Type Type)> GetTypeServices()
        => _typeServices.ToImmutableList();

    public IEnumerable<(Type As, object Instance)> GetInstanceServices()
        => _instanceServices.ToImmutableList();
}
