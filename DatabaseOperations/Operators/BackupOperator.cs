using System;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Executors;
using DatabaseOperations.Factories;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.Operators
{
    /// <summary>
    /// This is the 'backup' class of operators.
    /// </summary>
    public class BackupOperator : IBackupOperator
    {
        /// <summary>
        /// The <see langword="internal"/> constructor used for unit tests.
        /// </summary>
        /// <param name="executor"> The sql executor that will execute the commands for the operations. </param>
        internal BackupOperator(ISqlExecutor executor)
        {
            _sqlExecutor = executor;
        }

        /// <summary>
        /// Initialises a new instance of the BackupOperator. 
        /// </summary>
        public BackupOperator()
        {
            _sqlExecutor = new SqlExecutor(new SqlServerConnectionFactory());
        }

        private readonly ISqlExecutor _sqlExecutor;
        private const string ExecutionCancelledMessage = "Cancel called on the token.";

        /// <summary>
        /// Uses the <paramref name="options" /> defined by the user to start
        /// the backup process.
        /// </summary>
        /// <param name="options">
        /// The connection options defined by the consumer of the method.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// The database exception was not added to the 'Message' list.
        /// </exception>
        /// <returns>
        /// The result of the backup operation.
        /// </returns>
        public OperationResult BackupDatabase(ConnectionOptions options)
        {
            var result = new OperationResult();

            if (!options.IsValid())
            {
                result.Messages = options.Messages;
                return result;
            }

            result = _sqlExecutor.ExecuteBackupPath(result, options);

            // If result of path execution is not true, we want to strip out the path, so we only have the backup name.
            if (!result.Result)
            {
                result.Messages.Add("Unable to check the path, reverting to default save path.");
                options.RemovePathFromBackupLocation();
            }

            result = _sqlExecutor.ExecuteBackupDatabase(result, options);

            return result;
        }

        /// <summary>
        /// Uses the <paramref name="options" /> defined by the user to start
        /// the backup process.
        /// This is the async version.
        /// </summary>
        /// <param name="options">
        /// The connection options defined by the consumer of the method.
        /// </param>
        /// <param name="token"> The cancellation token supplied by the calling application. </param>
        /// <exception cref="NotSupportedException">
        /// The database exception was not added to the 'Message' list.
        /// </exception>
        /// <returns>
        /// The result of the backup operation.
        /// </returns>
        public async Task<OperationResult> BackupDatabaseAsync(ConnectionOptions options, CancellationToken token = default)
        {
            var result = new OperationResult();

            if (!options.IsValid())
            {
                result.Messages = options.Messages;
                return result;
            }

            if (token.IsCancellationRequested)
            {
                result.Messages.Add(ExecutionCancelledMessage);
                return result;
            }

            result = await _sqlExecutor.ExecuteBackupPathAsync(result, options, token).ConfigureAwait(false);

            if (token.IsCancellationRequested)
            {
                result.Result = false;
                result.Messages.Add(ExecutionCancelledMessage);
                return result;
            }

            // If result of path execution is not true, we want to strip out the path, so we only have the backup name.
            if (!result.Result)
            {
                result.Messages.Add("Unable to check the path, reverting to default save path.");
                options.RemovePathFromBackupLocation();
            }
            
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, options, token).ConfigureAwait(false);

            if (!token.IsCancellationRequested) return result;

            result.Result = false;
            result.Messages.Add(ExecutionCancelledMessage);
            return result;
        }
    }
}
