namespace DatabaseOperations.Interfaces
{
    internal interface ISqlServerConnectionFactory
	{
        ISqlConnectionWrapper CreateConnection(string connectionString);
        ISqlCommandWrapper CreateCommand(string commandText, ISqlConnectionWrapper connection);
	}
}
