using System;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Operators;
using FluentAssertions;
using Microsoft.SqlServer.Management.Smo;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace DatabaseOperations.Tests.Operators
{
    public class BackupOperatorTests
    {
        public BackupOperatorTests()
        {
            _sqlServerConnectionFactory = Substitute.For<ISqlServerConnectionFactory>();
            _backupWrapper = Substitute.For<IBackupWrapper>();
            var sqlConnectionWrapper = Substitute.For<ISqlConnectionWrapper>();
            var serverConnectionWrapper = Substitute.For<IServerConnectionWrapper>();
            _serverWrapper = Substitute.For<IServerWrapper>();
            _databaseUtility = new BackupOperator(_sqlServerConnectionFactory, Substitute.For<IConsoleWrapper>());

            _sqlServerConnectionFactory.GenerateBackupWrapper(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_backupWrapper);
            _sqlServerConnectionFactory.GenerateSqlConnectionWrapper(Arg.Any<string>()).Returns(sqlConnectionWrapper);
            _sqlServerConnectionFactory
                .GenerateServerConnectionWrapper(sqlConnectionWrapper)
                .Returns(serverConnectionWrapper);
            _sqlServerConnectionFactory.GenerateServerWrapper(serverConnectionWrapper)
                .Returns(_serverWrapper);
		}

		private readonly ISqlServerConnectionFactory _sqlServerConnectionFactory;
		private readonly IBackupWrapper _backupWrapper;
		private readonly IServerWrapper _serverWrapper;
		private readonly IBackupOperator _databaseUtility;

		[Fact]
		public void TestBackupDatabaseWithValidOptionsReturnsTrue()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
			_serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeTrue();
		}

		[Fact]
		public void TestBackupDatabaseWithGenerateBackupThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
			_serverWrapper.GetServer().Returns(server);
			_sqlServerConnectionFactory.GenerateBackupWrapper(Arg.Any<string>(), Arg.Any<string>())
				.Throws(new Exception("Unable to generate the backup wrapper."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithGenerateSqlConnectionThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
			_sqlServerConnectionFactory.GenerateSqlConnectionWrapper(Arg.Any<string>())
				.Throws(new Exception("Unable to generate the sql connection wrapper."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithGenerateServerConnectionThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
			_sqlServerConnectionFactory.GenerateServerConnectionWrapper(Arg.Any<ISqlConnectionWrapper>())
				.Throws(new Exception("Unable to generate the server connection wrapper."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithGenerateServerThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
			_sqlServerConnectionFactory.GenerateServerWrapper(Arg.Any<IServerConnectionWrapper>())
				.Throws(new Exception("Unable to generate the server wrapper."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithSetTimeoutThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			_serverWrapper.GetServer().Throws(new Exception("Unable to get the server to set the timeout."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithAddDeviceThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
            _backupWrapper
                .When(w => w.AddDevice(Arg.Any<BackupDeviceItem>()))
                .Do(w => throw new Exception("Unable to add a new device."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithRemoveDeviceThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
			_backupWrapper
                .When(w => w.RemoveDevice(Arg.Any<BackupDeviceItem>()))
                .Do(w => throw new Exception("Unable to remove the device."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithSqlBackupThrowsExceptionReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);
			_backupWrapper
                .When(w => w.SqlBackup(Arg.Any<IServerWrapper>()))
                .Do(w => throw new Exception("Unable to backup the database."));

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithInvalidConnectionStringReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionDetails.ConnectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithoutSettingConnectionStringReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithInvalidDatabaseReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithoutSettingDatabaseReturnsFalse()
		{
			// Arrange.
			const string backupLocation = "C:\\Olmec\\Maintenance\\Database Backups\\";
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithInvalidLocationReturnsFalse()
		{
			// Arrange.
			var backupLocation = string.Empty;
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseBackupLocation(backupLocation);
			_databaseUtility.UseConnectionString(connectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}

		[Fact]
		public void TestBackupDatabaseWithoutSettingLocationReturnsFalse()
		{
			// Arrange.
			const string connectionString =
				"server=(localdb)\\MSSQLLocalDB; Database=Banana; Integrated Security=SSPI; Connect Timeout=10;";
			var connectionDetails = new ConnectionDetails
			{
				ServerName = "(localdb)\\MSSQLLocalDB",
				DatabaseName = "Banana",
				ConnectionString = connectionString,
				IsValid = true
			};
			var server = new Server(connectionDetails.ConnectionString);
            _serverWrapper.GetServer().Returns(server);

			_databaseUtility.UseDatabase(connectionDetails.DatabaseName);
			_databaseUtility.UseConnectionString(connectionString);

			// Act.
			var result = _databaseUtility.BackupDatabase();

			// Assert.
			result.Should().BeFalse();
		}
	}
}
