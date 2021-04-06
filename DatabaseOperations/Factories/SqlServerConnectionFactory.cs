using System;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Operators;
using DatabaseOperations.Wrappers;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Factories
{
    public class SqlServerConnectionFactory : ISqlServerConnectionFactory
	{
        public IBackupOperator GenerateBackupOperator(IConsoleWrapper console)
        {
            IBackupOperator backupOperator = new BackupOperator(this, console);
            return backupOperator;
        }

        public IBackupWrapper GenerateBackupWrapper(string databaseName, string connectionString)
        {
            IBackupWrapper backupWrapper = new BackupWrapper
            {
                ActionType = BackupActionType.Database,
                BackupSetDescription = $"Backup of: {databaseName} using connection: {connectionString} on {DateTime.Now.ToShortDateString()}.",
                Database = databaseName,
                BackupSetName = "Full Backup",
                Initialize = true,
                Checksum = true,
                ContinueAfterError = true,
                Incremental = false,
                LogTruncation = BackupTruncateLogType.Truncate,
                FormatMedia = false
            };

            return backupWrapper;
        }

        public ISqlConnectionWrapper GenerateSqlConnectionWrapper(string connectionString)
        {
            ISqlConnectionWrapper sqlConnectionWrapper = new SqlConnectionWrapper(connectionString);
            return sqlConnectionWrapper;
        }

        public IServerConnectionWrapper GenerateServerConnectionWrapper(ISqlConnectionWrapper connectionWrapper)
        {
            IServerConnectionWrapper serverConnectionWrapper = new ServerConnectionWrapper(connectionWrapper);
            return serverConnectionWrapper;
        }

        public IServerWrapper GenerateServerWrapper(IServerConnectionWrapper connectionWrapper)
        {
            IServerWrapper serverWrapper = new ServerWrapper(connectionWrapper);
            return serverWrapper;
        }
	}
}
