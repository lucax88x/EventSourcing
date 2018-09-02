using System.Threading.Tasks;

namespace EventSourcing.Infrastructure.Write
{
    public class Migrator
    {
        private readonly ConnectionFactory _connectionFactory;

        public Migrator(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Up()
        {
            using (var session = _connectionFactory.Connect())
            {
                var dropStatement = session.Prepare("DROP TABLE IF EXISTS es.event");
                session.Execute(dropStatement.Bind());
                
                var statement = session.Prepare("CREATE TABLE es.event (aggregateId UUID, createdWhen TIMESTAMP, type TEXT, payload TEXT, PRIMARY KEY (aggregateId, createdWhen)) WITH CLUSTERING ORDER BY (createdWhen ASC)");
                session.Execute(statement.Bind());
            }
        }
    }
}