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

        OperationResult ExecuteBackupPath(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties);

        Task<OperationResult> ExecuteBackupPathAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token);

        Task<OperationResult> ExecuteBackupPathAsync(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token);

        OperationResult ExecuteBackupDatabase(
            OperationResult result,
            ConnectionOptions options);

        OperationResult ExecuteBackupDatabase(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties);

        Task<OperationResult> ExecuteBackupDatabaseAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token);

        Task<OperationResult> ExecuteBackupDatabaseAsync(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token);
    }
}