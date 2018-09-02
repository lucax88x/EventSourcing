using Autofac;

namespace EventSourcing.Infrastructure.Common
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Serializer>().AsSelf();
        }
    }
}