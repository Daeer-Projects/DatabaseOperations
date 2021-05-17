using System;
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
EXEC master.dbo.xp_create_subdir @BackupPath;

BACKUP DATABASE @DatabaseName
TO DISK = @BackupLocation
WITH
    NAME = @DatabaseName,
    DESCRIPTION = @BackupDescription
;
";

        private readonly ISqlServerConnectionFactory _sqlCreator;

        public bool BackupDatabase(ConnectionDetails details)
        {
            var result = false;
            try
            {
                using (var connection = _sqlCreator.CreateConnection(details.ConnectionString))
                {
                    using (var command = _sqlCreator.CreateCommand(SqlScriptTemplate, connection))
                    {
                        command.AddParameters(details.Parameters());
                        command.SetCommandTimeout(details.CommandTimeout);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                result = true;
            }
            catch (DbException exception)
            {
                Console.WriteLine($"Backing up the database failed due to an exception.  Exception: {exception.Message}");
            }

            return result;
        }
	}
}
