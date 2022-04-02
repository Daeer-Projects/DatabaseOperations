using System;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;

namespace DatabaseOperations.Interfaces
{
    /// <summary>
    /// This is the 'backup' <see langword="interface"/> for the operators.
    /// </summary>
    public interface IBackupOperator
    {
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
        OperationResult BackupDatabase(ConnectionOptions options);

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
        Task<OperationResult> BackupDatabaseAsync(ConnectionOptions options, CancellationToken token);
    }
}
