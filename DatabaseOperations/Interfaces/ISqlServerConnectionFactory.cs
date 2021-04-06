namespace DatabaseOperations.Interfaces
{
    public interface ISqlServerConnectionFactory
	{
        IBackupOperator GenerateBackupOperator(IConsoleWrapper console);
        IBackupWrapper GenerateBackupWrapper(string databaseName, string connectionString);
        ISqlConnectionWrapper GenerateSqlConnectionWrapper(string connectionString);
        IServerConnectionWrapper GenerateServerConnectionWrapper(ISqlConnectionWrapper connectionWrapper);
        IServerWrapper GenerateServerWrapper(IServerConnectionWrapper connectionWrapper);
	}
}
