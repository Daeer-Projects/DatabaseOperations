namespace DatabaseOperations.Interfaces
{
    public interface IBackupOperator
	{
        IBackupOperator UseConnectionString(string connectionString);
        IBackupOperator UseDatabase(string databaseName);
        IBackupOperator UseBackupLocation(string backupLocation);
        bool BackupDatabase();
	}
}
