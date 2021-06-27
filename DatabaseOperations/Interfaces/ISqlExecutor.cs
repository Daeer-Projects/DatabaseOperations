using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    internal interface ISqlExecutor
    {
        OperationResult ExecuteBackupPath(OperationResult result, ConnectionOptions options);
        OperationResult ExecuteBackupDatabase(OperationResult result, ConnectionOptions options);
    }
}