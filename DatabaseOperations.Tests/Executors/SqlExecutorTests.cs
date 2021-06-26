using System.Collections.Generic;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Executors;
using DatabaseOperations.Interfaces;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Executors
{
    public class SqlExecutorTests
    {
        public SqlExecutorTests()
        {
            var creator = Substitute.For<ISqlServerConnectionFactory>();
            _connection = Substitute.For<ISqlConnectionWrapper>();
            _command = Substitute.For<ISqlCommandWrapper>();

            creator.CreateConnection(Arg.Any<string>()).Returns(_connection);
            creator.CreateCommand(Arg.Any<string>(), _connection).Returns(_command);
            _sqlExecutor = new SqlExecutor(creator);
        }

        private const string BackupPath = @"C:\Database Backups\";
        private readonly ISqlConnectionWrapper _connection;
        private readonly ISqlCommandWrapper _command;
        private readonly ISqlExecutor _sqlExecutor;

        [Fact]
        public void TestBackupPathWithValidDetailsReturnsTrue()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "127.0.0.1", "Thing");
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void TestBackupPathWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            var result = new OperationResult();

            var expectedMessages = new List<string>
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public void TestBackupDatabaseWithValidDetailsReturnsTrue()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "127.0.0.1", "Thing");
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void TestBackupDatabaseWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            var result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            var result = new OperationResult();

            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        private static ConnectionOptions GetConnectionOptions(string serverParameter, string databaseParameter, string serverName, string databaseName)
        {
            var connectionString = $"{serverParameter}={serverName};{databaseParameter}={databaseName};User Id=sa;Password=password;Connect Timeout=10;";
            return new ConnectionOptions(connectionString, BackupPath);
        }
    }
}
