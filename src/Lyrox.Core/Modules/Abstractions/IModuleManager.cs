namespace Lyrox.Core.Modules.Abstractions
{
    public interface IModuleManager
    {
        void RegisterModule<T>() where T : IModule, new();
    }
}