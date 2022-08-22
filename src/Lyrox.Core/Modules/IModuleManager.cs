namespace Lyrox.Core.Modules
{
    public interface IModuleManager
    {
        void RegisterModule<T>() where T : IModule, new();
    }
}