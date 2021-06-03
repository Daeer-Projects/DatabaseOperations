using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;

namespace DatabaseOperations.Factories
{
    internal class SqlServerConnectionFactory : ISqlServerConnectionFactory
	{
        public ISqlConnectionWrapper CreateConnection(string connectionString)
        {
            return new SqlConnectionWrapper(connectionString);
        }

        public ISqlCommandWrapper CreateCommand(string commandText, ISqlConnectionWrapper connection)
        {
            return new SqlCommandWrapper(commandText, connection);
        }
	}
}
