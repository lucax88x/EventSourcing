using Autofac;
using MediatR;

namespace EventSourcing.Application
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Infrastructure.Write.Module());

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder
                .Register<ServiceFactory >(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.TryResolve(t, out var o) ? o : null;
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountService>().AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterType<EventStore>().AsSelf();
        }
    }
}