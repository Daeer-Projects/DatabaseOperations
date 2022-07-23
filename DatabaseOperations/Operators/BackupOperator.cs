namespace DatabaseOperations.Operators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;
    using Executors;
    using Extensions;
    using Factories;
    using Interfaces;
    using Options;
    using Services;

    /// <summary>
    ///     This is the 'backup' class of operators.
    /// </summary>
    public class BackupOperator : IBackupOperator
    {
        /// <summary>
        ///     The <see langword="internal" /> constructor used for unit tests.
        /// </summary>
        /// <param name="executor"> The sql executor that will execute the commands for the operations. </param>
        internal BackupOperator(ISqlExecutor executor)
        {
            sqlExecutor = executor;
        }

        /// <summary>
        ///     Initialises a new instance of the BackupOperator.
        /// </summary>
        public BackupOperator()
        {
            sqlExecutor = new SqlExecutor(new SqlServerConnectionFactory());
        }

        private readonly ISqlExecutor sqlExecutor;

        /// <summary>
        ///     Uses the <paramref name="options" /> defined by the user to start
        ///     the backup process.
        /// </summary>
        /// <param name="options">
        ///     The connection options defined by the consumer of the method.
        /// </param>
        /// <exception cref="NotSupportedException">
        ///     The database exception was not added to the 'Message' list.
        /// </exception>
        /// <returns>
        ///     The result of the backup operation.
        /// </returns>
        public OperationResult BackupDatabase(ConnectionOptions options)
        {
            OperationResult result = new OperationResult()
                .ValidateConnectionOptions(options)
                .ExecuteBackupPath(options, sqlExecutor)
                .CheckBackupPathExecution(options)
                .ExecuteBackup(options, sqlExecutor);

            return result;
        }

        /// <summary>
        ///     Uses the <paramref name="options" /> defined by the user to start
        ///     the backup process.
        ///     This is the async version.
        /// </summary>
        /// <param name="options">
        ///     The connection options defined by the consumer of the method.
        /// </param>
        /// <param name="token"> The cancellation token supplied by the calling application. </param>
        /// <exception cref="NotSupportedException">
        ///     The database exception was not added to the 'Message' list.
        /// </exception>
        /// <returns>
        ///     The result of the backup operation.
        /// </returns>
        public async Task<OperationResult> BackupDatabaseAsync(
            ConnectionOptions options,
            CancellationToken token = default)
        {
            OperationResult result = await new OperationResult()
                .ValidateConnectionOptionsAsync(options)
                .CheckForCancellation(token)
                .ExecuteBackupPathAsync(options, token, sqlExecutor)
                .CheckForCancellation(token)
                .CheckBackupPathExecutionAsync(options, token)
                .CheckForCancellation(token)
                .ExecuteBackupAsync(options, token, sqlExecutor)
                .CheckForCancellation(token)
                .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        ///     Uses the <paramref name="options" /> defined by the user to start
        ///     the backup process.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string defined by the consumer of the method.
        /// </param>
        /// <param name="options">
        ///     The defined options required by the consumer of the method.
        /// </param>
        /// <exception cref="NotSupportedException">
        ///     The database exception was not added to the 'Message' list.
        /// </exception>
        /// <returns>
        ///     The result of the backup operation.
        /// </returns>
        public OperationResult BackupDatabase(
            string connectionString,
            Action<OperatorOptions>? options = null)
        {
            OperatorOptions optionsToUse = new();
            options?.Invoke(optionsToUse);
            ConnectionProperties properties = ConnectionStringService.ExtractConnectionParameters(connectionString);

            OperationResult result = new OperationResult()
                .ValidateConnectionProperties(properties);
            //.ValidateConnectionOptions(options)
            //.ExecuteBackupPath(options, sqlExecutor)
            //.CheckBackupPathExecution(options)
            //.ExecuteBackup(options, sqlExecutor);

            return result;
        }
    }
}