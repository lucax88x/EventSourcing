using System;
using Cassandra;

namespace EventSourcing.Infrastructure.Write
{
    public class ConnectionFactory
    {
        public ISession Connect()
        {
            var cluster = Cluster.Builder()
                .AddContactPoints("127.0.0.1")
                .WithDefaultKeyspace("es")
                .Build();

            return cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
        }
    }
}