using System;
using System.Data.Common;
using DatabaseOperations.DataTransferObjects;
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
        /// <param name="creator"> The connection factory that will allow the creation of the SQL classes. </param>
        internal BackupOperator(ISqlServerConnectionFactory creator)
        {
            _sqlCreator = creator;
        }

        /// <summary>
        /// Initialises a new instance of the BackupOperator. 
        /// </summary>
        public BackupOperator()
        {
            _sqlCreator = new SqlServerConnectionFactory();
        }

        private const string SqlScriptTemplate = @"
IF (@BackupPath IS NOT NULL AND @BackupPath <> '')
BEGIN
    EXEC master.dbo.xp_create_subdir @BackupPath;
END

BACKUP DATABASE @DatabaseName
TO DISK = @BackupLocation
WITH
    NAME = @DatabaseName,
    DESCRIPTION = @BackupDescription
;
";

        private readonly ISqlServerConnectionFactory _sqlCreator;

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
            
            try
            {
                using (var connection = _sqlCreator.CreateConnection(options.ConnectionString))
                {
                    using (var command = _sqlCreator.CreateCommand(SqlScriptTemplate, connection))
                    {
                        command.AddParameters(options.Parameters());
                        command.SetCommandTimeout(options.CommandTimeout);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                
                result.Result = true;
            }
            catch (DbException exception)
            {
                result.Messages.Add($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }
	}
}
