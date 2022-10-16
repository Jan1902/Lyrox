namespace Lyrox.Framework.Core.Modules.Abstractions
{
    public interface IModuleManager
    {
        void RegisterModule<T>() where T : IModule, new();
        void RegisterModule(Type moduleType);
    }
}
