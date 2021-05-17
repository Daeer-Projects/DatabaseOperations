using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Operators;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Operators
{
    public class BackupOperatorTests
    {
        public BackupOperatorTests()
        {
            var creator = Substitute.For<ISqlServerConnectionFactory>();
            _connection = Substitute.For<ISqlConnectionWrapper>();
            _command = Substitute.For<ISqlCommandWrapper>();

            creator.CreateConnection(Arg.Any<string>()).Returns(_connection);
            creator.CreateCommand(Arg.Any<string>(), _connection).Returns(_command);
            _backupOperator = new BackupOperator(creator);
		}

        private const string BackupPath = @"C:\Database Backups\";
        private readonly ISqlConnectionWrapper _connection;
        private readonly ISqlCommandWrapper _command;
		private readonly IBackupOperator _backupOperator;

        [Fact]
        public void TestBackupWithValidDetailsReturnsTrue()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "127.0.0.1", "Thing");

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Should().BeTrue();
        }

        [Fact]
        public void TestBackupWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _connection
                .When(c => c.Open())
                .Do(c => throw  new DbTestException("Server is not correct!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(c => throw new DbTestException("Command is not working!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(c => throw new DbTestException("Execute is not working!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Should().BeFalse();
        }

        private static ConnectionOptions GetConnectionOptions(string serverParameter, string databaseParameter, string serverName, string databaseName)
        {
            string connectionString = $"{serverParameter}={serverName};{databaseParameter}={databaseName};User Id=sa;Password=password;Connect Timeout=10;";
            return new ConnectionOptions(connectionString, BackupPath);
        }
	}
}
