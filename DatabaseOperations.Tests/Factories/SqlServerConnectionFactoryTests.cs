using DatabaseOperations.Factories;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Operators;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Factories
{
    public class SqlServerConnectionFactoryTests
	{
		private readonly SqlServerConnectionFactory _connectionFactory = new SqlServerConnectionFactory();

		[Fact]
		public void TestGenerateOperatorReturnsOperator()
		{
			// Arrange.
			// Act.
            var backupOperator = _connectionFactory.GenerateBackupOperator(Substitute.For<IConsoleWrapper>());

			// Assert.
			backupOperator.Should().NotBeNull();
			backupOperator.Should().BeOfType<BackupOperator>();
		}

		[Fact]
		public void TestGenerateBackupReturnsBackup()
		{
			// Arrange.
			// Act.
			var backup = _connectionFactory.GenerateBackupWrapper("bananas", "server=127.0.0.1; database=bananas;");

			// Assert.
			backup.Should().NotBeNull();
			backup.Should().BeOfType<BackupWrapper>();
		}

		[Fact]
		public void TestGenerateSqlConnectionReturnsSqlConnection()
		{
			// Arrange.
			// Act.
			var connectionWrapper = _connectionFactory.GenerateSqlConnectionWrapper("server=127.0.0.1; database=bananas;");

			// Assert.
			connectionWrapper.Should().NotBeNull();
			connectionWrapper.Should().BeOfType<SqlConnectionWrapper>();
		}

		[Fact]
		public void TestGenerateServerConnectionReturnsServerConnection()
		{
			// Arrange.
			var connectionWrapper = _connectionFactory.GenerateSqlConnectionWrapper("server=127.0.0.1; database=bananas;");

			// Act.
			var serverConnection = _connectionFactory.GenerateServerConnectionWrapper(connectionWrapper);

			// Assert.
			serverConnection.Should().NotBeNull();
			serverConnection.Should().BeOfType<ServerConnectionWrapper>();
		}

		[Fact]
		public void TestGenerateServerReturnsServer()
		{
			// Arrange.
			var connectionWrapper = _connectionFactory.GenerateSqlConnectionWrapper("server=127.0.0.1; database=bananas;");
			var serverConnection = _connectionFactory.GenerateServerConnectionWrapper(connectionWrapper);

			// Act.
			var server = _connectionFactory.GenerateServerWrapper(serverConnection);

			// Assert.
			server.Should().NotBeNull();
			server.Should().BeOfType<ServerWrapper>();
		}
	}
}
