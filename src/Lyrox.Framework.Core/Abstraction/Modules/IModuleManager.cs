namespace Lyrox.Framework.Core.Abstraction.Modules;

public interface IModuleManager
{
    void RegisterModule<T>() where T : IModule, new();
}
