using System.Data.Common;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.Operators
{
    public class BackupOperator : IBackupOperator
    {
        public BackupOperator(ISqlServerConnectionFactory creator)
        {
            _sqlCreator = creator;
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

        public OperationResult<bool> BackupDatabase(ConnectionOptions options)
        {
            var result = new OperationResult<bool>();

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
