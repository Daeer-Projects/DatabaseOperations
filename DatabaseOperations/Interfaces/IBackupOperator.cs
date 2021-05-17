using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    public interface IBackupOperator
	{
        bool BackupDatabase(ConnectionOptions options);
	}
}
