using System;
using System.Threading.Tasks;
using Autofac;
using EventSourcing.Infrastructure.Write;
using MediatR;
using Xunit;

namespace EventSourcing.Application.Test
{
    public class AccountServiceTest : IDisposable
    {
        private readonly IContainer _container;
        private readonly IMediator _mediator;

        public AccountServiceTest()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule(new Module());

            _container = cb.Build();

            _mediator = _container.Resolve<IMediator>();
            var migrator = _container.Resolve<Migrator>();

            migrator.Up();
        }

        [Fact]
        public async Task create_account()
        {
            await _mediator.Send(new Domain.Account.Commands.CreateAccount(Guid.NewGuid(), "test account"));
        }
        
        [Fact]
        public async Task update_name_on_account()
        {
            var id = Guid.NewGuid();
            await _mediator.Send(new Domain.Account.Commands.CreateAccount(id, "test account"));

            await _mediator.Send(new Domain.Account.Commands.SetAccountName(id, "name2"));
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}