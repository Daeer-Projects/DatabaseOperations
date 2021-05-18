using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    public interface IBackupOperator
	{
        OperationResult<bool> BackupDatabase(ConnectionOptions options);
	}
}
