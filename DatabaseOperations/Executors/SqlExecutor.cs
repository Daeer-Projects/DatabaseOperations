namespace DatabaseOperations.Executors
{
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;
    using Interfaces;

    internal class SqlExecutor : ISqlExecutor
    {
        internal SqlExecutor(ISqlServerConnectionFactory creator)
        {
            sqlCreator = creator;
        }

        private const string SqlScriptCreateBackupPathTemplate = @"
IF (@BackupPath IS NOT NULL AND @BackupPath <> '')
BEGIN
    EXEC master.dbo.xp_create_subdir @BackupPath;
END
;
";

        private const string SqlScriptBackupDatabaseTemplate = @"
BACKUP DATABASE @DatabaseName
TO DISK = @BackupLocation
WITH
    NAME = @DatabaseName,
    DESCRIPTION = @BackupDescription
;
";

        private readonly ISqlServerConnectionFactory sqlCreator;

        public OperationResult ExecuteBackupPath(
            OperationResult result,
            ConnectionOptions options)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(options.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptCreateBackupPathTemplate, connection);
                command.AddParameters(options.BackupParameters());
                command.SetCommandTimeout(options.CommandTimeout);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add(
                    $"Backup path folder check/create failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public OperationResult ExecuteBackupPath(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(connectionProperties.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptCreateBackupPathTemplate, connection);
                command.AddParameters(backupProperties.BackupParameters);
                command.SetCommandTimeout(backupProperties.CommandTimeout);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add(
                    $"Backup path folder check/create failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public async Task<OperationResult> ExecuteBackupPathAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(options.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptCreateBackupPathTemplate, connection);
                command.AddParameters(options.BackupParameters());
                command.SetCommandTimeout(options.CommandTimeout);
                await connection.OpenAsync(token)
                    .ConfigureAwait(false);
                await command.ExecuteNonQueryAsync(token)
                    .ConfigureAwait(false);
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add(
                    $"Backup path folder check/create failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public async Task<OperationResult> ExecuteBackupPathAsync(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(connectionProperties.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptCreateBackupPathTemplate, connection);
                command.AddParameters(backupProperties.BackupParameters);
                command.SetCommandTimeout(backupProperties.CommandTimeout);
                await connection.OpenAsync(token)
                    .ConfigureAwait(false);
                await command.ExecuteNonQueryAsync(token)
                    .ConfigureAwait(false);
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add(
                    $"Backup path folder check/create failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public OperationResult ExecuteBackupDatabase(
            OperationResult result,
            ConnectionOptions options)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(options.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptBackupDatabaseTemplate, connection);
                command.AddParameters(options.ExecutionParameters());
                command.SetCommandTimeout(options.CommandTimeout);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public OperationResult ExecuteBackupDatabase(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(connectionProperties.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptBackupDatabaseTemplate, connection);
                command.AddParameters(backupProperties.ExecutionParameters);
                command.SetCommandTimeout(backupProperties.CommandTimeout);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public async Task<OperationResult> ExecuteBackupDatabaseAsync(
            OperationResult result,
            ConnectionOptions options,
            CancellationToken token)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(options.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptBackupDatabaseTemplate, connection);
                command.AddParameters(options.ExecutionParameters());
                command.SetCommandTimeout(options.CommandTimeout);
                await connection.OpenAsync(token)
                    .ConfigureAwait(false);
                await command.ExecuteNonQueryAsync(token)
                    .ConfigureAwait(false);
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }

        public async Task<OperationResult> ExecuteBackupDatabaseAsync(
            OperationResult result,
            ConnectionProperties connectionProperties,
            BackupProperties backupProperties,
            CancellationToken token)
        {
            try
            {
                using ISqlConnectionWrapper connection = sqlCreator.CreateConnection(connectionProperties.ConnectionString);
                using ISqlCommandWrapper command = sqlCreator.CreateCommand(SqlScriptBackupDatabaseTemplate, connection);
                command.AddParameters(backupProperties.ExecutionParameters);
                command.SetCommandTimeout(backupProperties.CommandTimeout);
                await connection.OpenAsync(token)
                    .ConfigureAwait(false);
                await command.ExecuteNonQueryAsync(token)
                    .ConfigureAwait(false);
            }
            catch (DbException exception)
            {
                result.Result = false;
                result.Messages.Add($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }
    }
}