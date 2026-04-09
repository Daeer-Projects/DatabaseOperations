namespace DatabaseOperations.Factories
{
    using Interfaces;
    using Wrappers;

    internal sealed class SqlServerConnectionFactory : ISqlServerConnectionFactory
    {
        public ISqlConnectionWrapper CreateConnection(string connectionString)
        {
            return new SqlConnectionWrapper(connectionString);
        }

        public ISqlCommandWrapper CreateCommand(
            string commandText,
            ISqlConnectionWrapper connection)
        {
            return new SqlCommandWrapper(commandText, connection);
        }
    }
}