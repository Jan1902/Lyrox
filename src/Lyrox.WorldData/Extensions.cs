using Autofac;
using Lyrox.Core.Abstraction;

namespace Lyrox.WorldData
{
    public static class Extensions
    {
        public static ContainerBuilder AddWorldDataManager(this ContainerBuilder builder)
        {
            builder.RegisterType<WorldDataManager>().As<IWorldDataManager>();

            return builder;
        }
    }
}