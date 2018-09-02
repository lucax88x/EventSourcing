using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using EventSourcing.Domain;
using EventSourcing.Infrastructure.Common;

namespace EventSourcing.Infrastructure.Write
{
    public class WriteRepository
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly Serializer _serializer;

        public WriteRepository(ConnectionFactory connectionFactory, Serializer serializer)
        {
            _connectionFactory = connectionFactory;
            _serializer = serializer;
        }

        public async Task Save(Guid id, IEnumerable<Base.Event> events)
        {
            using (var session = _connectionFactory.Connect())
            {
                var ps = await session.PrepareAsync(
                    "INSERT INTO es.event (aggregateId, createdWhen, type, payload) VALUES (?, ?, ?, ?)");

                var batch = new BatchStatement();

                foreach (var evt in events)
                    batch.Add(ps.Bind(id, evt.CreatedWhen, evt.GetType().AssemblyQualifiedName,
                        _serializer.Serialize(evt)));

                await session.ExecuteAsync(batch);
            }
        }

        public async Task<IEnumerable<Base.Event>> GetById(Guid id)
        {
            using (var session = _connectionFactory.Connect())
            {
                var ps = await session.PrepareAsync("SELECT type, payload FROM es.event WHERE aggregateId = ?");

                var rs = await session.ExecuteAsync(ps.Bind(id));

                var rows = rs.ToList();

                if (!rows.Any()) throw new Exception("not found");

                var events = new List<Base.Event>();
                foreach (var row in rows)
                {
                    var type = row.GetValue<string>("type");
                    var payload = row.GetValue<string>("payload");

                    var deserialized = _serializer.Deserialize(Type.GetType(type), payload);
                    if (deserialized is Base.Event evt) events.Add(evt);
                }
                return events;
            }
        }
    }
}