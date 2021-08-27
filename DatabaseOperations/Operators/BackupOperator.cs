using System;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Executors;
using DatabaseOperations.Extensions;
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
            var result = new OperationResult()
                .ValidateConnectionOptions(options)
                .ExecuteBackupPath(options, _sqlExecutor)
                .CheckBackupPathExecution(options)
                .ExecuteBackup(options, _sqlExecutor);
            
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
            var result = await new OperationResult()
                .ValidateConnectionOptionsAsync(options)
                .CheckForCancellation(token)
                .ExecuteBackupPathAsync(options, token, _sqlExecutor)
                .CheckForCancellation(token)
                .CheckBackupPathExecutionAsync(options, token)
                .CheckForCancellation(token)
                .ExecuteBackupAsync(options, token, _sqlExecutor)
                .CheckForCancellation(token)
                .ConfigureAwait(false);

            return result;
        }
    }
}
