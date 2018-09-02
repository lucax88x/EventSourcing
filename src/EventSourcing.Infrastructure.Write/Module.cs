using Autofac;

namespace EventSourcing.Infrastructure.Write
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Common.Module());

            builder.RegisterType<ConnectionFactory>().AsSelf();
            builder.RegisterType<Migrator>().AsSelf();
            builder.RegisterType<WriteRepository>().AsSelf();
        }
    }
}