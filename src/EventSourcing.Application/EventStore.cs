using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventSourcing.Domain;
using EventSourcing.Infrastructure.Write;
using MediatR;

namespace EventSourcing.Application
{
    public class EventStore
    {
        private readonly WriteRepository _writeRepository;
        private readonly IMediator _mediator;

        public EventStore(WriteRepository writeRepository, IMediator mediator)
        {
            _writeRepository = writeRepository;
            _mediator = mediator;
        }

        public async Task Save<T>(T aggregate, CancellationToken cancellationToken) where T : Base.AggregateRoot
        {
            var events = aggregate.GetUncommittedChanges();

            if (!events.Any()) throw new Exception("nothing to save");

            await _writeRepository.Save(aggregate.Id, events);

            foreach (var evt in events)
                await _mediator.Publish(evt, cancellationToken);
        }

        public async Task<T> GetById<T>(Guid id) where T : Base.AggregateRoot, new()
        {
            var events = await _writeRepository.GetById(id);

            var aggregateRoot = new T();

            aggregateRoot.LoadsFromHistory(events);

            return aggregateRoot;
        }
    }
}