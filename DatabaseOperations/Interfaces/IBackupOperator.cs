using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    public interface IBackupOperator
	{
        bool BackupDatabase(ConnectionDetails details);
	}
}
