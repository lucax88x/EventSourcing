using System.Threading;
using System.Threading.Tasks;
using EventSourcing.Domain.Account;
using MediatR;

namespace EventSourcing.Application
{
    public class AccountService :
        IRequestHandler<Commands.CreateAccount>,
        IRequestHandler<Commands.SetAccountName>
    {
        private readonly EventStore _eventStore;

        public AccountService(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Unit> Handle(Commands.CreateAccount request, CancellationToken cancellationToken)
        {
            var account = Account.Create(request.Id, request.Name);

            await _eventStore.Save(account, cancellationToken);
                
            return Unit.Value;
        }

        public async Task<Unit> Handle(Commands.SetAccountName request, CancellationToken cancellationToken)
        {
            var account = await _eventStore.GetById<Account>(request.Id);
             
            account.SetName(request.Name);

            await _eventStore.Save(account, cancellationToken);

            return Unit.Value;
        }
    }
}