using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.Extensions
{
    internal static class OperationResultExtensions
    {
        private const string ExecutionCancelledMessage = "Cancel called on the token.";
        private const string BackupPathCheckFailureMessage = "Unable to check the path, reverting to default save path.";

        internal static OperationResult ValidateConnectionOptions(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (options.IsValid()) return operationResult;

            operationResult.Result = false;
            operationResult.Messages = options.Messages;
            return operationResult;
        }

        internal static OperationResult ExecuteBackupPath(
            this OperationResult operationResult,
            ConnectionOptions options,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result ? operationResult : sqlExecutor.ExecuteBackupPath(operationResult, options);
        }

        internal static OperationResult CheckBackupPathExecution(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (!operationResult.Messages.Any()) return operationResult;

            operationResult.Result = true;
            operationResult.Messages.Add(BackupPathCheckFailureMessage);
            options.RemovePathFromBackupLocation();

            return operationResult;
        }

        internal static OperationResult ExecuteBackup(
            this OperationResult operationResult,
            ConnectionOptions options,
            ISqlExecutor sqlExecutor)
        {
            return !operationResult.Result ? operationResult : sqlExecutor.ExecuteBackupDatabase(operationResult, options);
        }

        internal static async Task<OperationResult> ValidateConnectionOptionsAsync(
            this OperationResult operationResult,
            ConnectionOptions options)
        {
            if (options.IsValid()) return await Task.FromResult(operationResult).ConfigureAwait(false);

            operationResult.Result = false;
            operationResult.Messages = options.Messages;
            return await Task.FromResult(operationResult).ConfigureAwait(false);
        }

        internal static async Task<OperationResult> CheckForCancellation(
            this Task<OperationResult> operationResult,
            CancellationToken token)
        {
            if (!token.IsCancellationRequested) return await operationResult.ConfigureAwait(false);

            var result = await operationResult.ConfigureAwait(false);
            result.Result = false;
            if (!result.Messages.Contains(ExecutionCancelledMessage)) result.Messages.Add(ExecutionCancelledMessage);
            return await Task.FromResult(result).ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupPathAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            var result = await operationResult.ConfigureAwait(false);
            return !result.Result
                ? await Task.FromResult(result).ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupPathAsync(result, options, token).ConfigureAwait(false);
        }

        internal static async Task<OperationResult> CheckBackupPathExecutionAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token)
        {
            var result = await operationResult.ConfigureAwait(false);
            if (!result.Messages.Any() || token.IsCancellationRequested) return await Task.FromResult(result).ConfigureAwait(false);

            result.Result = true;
            result.Messages.Add(BackupPathCheckFailureMessage);
            options.RemovePathFromBackupLocation();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        internal static async Task<OperationResult> ExecuteBackupAsync(
            this Task<OperationResult> operationResult,
            ConnectionOptions options,
            CancellationToken token,
            ISqlExecutor sqlExecutor)
        {
            var result = await operationResult.ConfigureAwait(false);

            return !result.Result
                ? await Task.FromResult(result).ConfigureAwait(false)
                : await sqlExecutor.ExecuteBackupDatabaseAsync(result, options, token).ConfigureAwait(false);
        }
    }
}
