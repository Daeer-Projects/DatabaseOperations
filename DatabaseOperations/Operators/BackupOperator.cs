using System;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Validators;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseOperations.Operators
{
    public class BackupOperator : IBackupOperator
    {
        public BackupOperator(ISqlServerConnectionFactory factory, IConsoleWrapper consoleWrapper)
		{
			_connectionFactory = factory;
            _backupOptions = new BackupOptions();
            _console = consoleWrapper;
        }

        private readonly BackupOptions _backupOptions;
		private readonly ISqlServerConnectionFactory _connectionFactory;
		private readonly IConsoleWrapper _console;

		public IBackupOperator UseConnectionString(string connectionString)
		{
			_backupOptions.ConnectionString = connectionString;
			return this;
		}

		public IBackupOperator UseDatabase(string databaseName)
		{
			_backupOptions.DatabaseName = databaseName;
			return this;
		}

		public IBackupOperator UseBackupLocation(string backupLocation)
		{
			_backupOptions.Destination = backupLocation;
			return this;
		}

		public bool BackupDatabase()
		{
			var result = false;

			try
			{
				var areOptionsValid = _backupOptions.CheckValidation(new BackupOptionsValidator());
				if (areOptionsValid.IsValid)
				{
					var sqlBackup = _connectionFactory.GenerateBackupWrapper(_backupOptions.DatabaseName,
						_backupOptions.ConnectionString);

					var deviceItem =
						new BackupDeviceItem(
							$"{_backupOptions.Destination}{_backupOptions.DatabaseName}_Full_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.bak",
							DeviceType.File);

					var sqlConnection =
						_connectionFactory.GenerateSqlConnectionWrapper(_backupOptions.ConnectionString);
					var connection =
						_connectionFactory.GenerateServerConnectionWrapper(sqlConnection);

					var sqlServer = _connectionFactory.GenerateServerWrapper(connection);

					sqlServer.GetServer().ConnectionContext.StatementTimeout = 60 * 60;

					sqlBackup.AddDevice(deviceItem);
					sqlBackup.SqlBackup(sqlServer);
					sqlBackup.RemoveDevice(deviceItem);
					result = true;
				}
				else
				{
					var validationErrors = string.Join(", ", areOptionsValid.Errors);
					_console.WriteLine($"Backup Options failed validation: {validationErrors}");
				}
			}
			catch (Exception exception)
			{
				_console.WriteLine($"Backing up the database failed due to an exception.  Exception: {exception}");
			}

			return result;
		}
	}
}
