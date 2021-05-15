namespace DatabaseOperations.Interfaces
{
    public interface ISqlServerConnectionFactory
	{
        ISqlConnectionWrapper CreateConnection(string connectionString);
        ISqlCommandWrapper CreateCommand(string commandText, ISqlConnectionWrapper connection);
	}
}
