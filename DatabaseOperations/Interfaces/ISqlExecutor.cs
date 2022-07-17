namespace DatabaseOperations.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;

    internal interface ISqlExecutor
    {
        OperationResult ExecuteBackupPath(
            OperationResult result,
            ConnectionOptions options);

        Task<OperationResult> ExecuteBackupPathAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token);

        OperationResult ExecuteBackupDatabase(
            OperationResult result,
            ConnectionOptions options);

        Task<OperationResult> ExecuteBackupDatabaseAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token);
    }
}