namespace DatabaseOperations.Tests.Executors
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Executors;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.Data.SqlClient;
    using NSubstitute;
    using Xunit;

    public class SqlExecutorTests
    {
        public SqlExecutorTests()
        {
            ISqlServerConnectionFactory? creator = Substitute.For<ISqlServerConnectionFactory>();
            _connection = Substitute.For<ISqlConnectionWrapper>();
            _command = Substitute.For<ISqlCommandWrapper>();

            creator.CreateConnection(Arg.Any<string>())
                .Returns(_connection);
            creator.CreateCommand(Arg.Any<string>(), _connection)
                .Returns(_command);
            _sqlExecutor = new SqlExecutor(creator);
        }

        private const string BackupPath = @"C:\Database Backups\";
        private readonly ISqlCommandWrapper _command;
        private readonly ISqlConnectionWrapper _connection;
        private readonly ISqlExecutor _sqlExecutor;

        [Fact]
        public void TestBackupPathWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupPathAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestBackupPathWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupPathAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupPathAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupPathAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            List<string> expectedMessages = new List<string>
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = _sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Messages.Should()
                .HaveSameCount(expectedMessages);
            result.Messages.Should()
                .Equal(
                    expectedMessages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupPathAsyncWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            List<string> expectedMessages = new List<string>
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = await _sqlExecutor.ExecuteBackupPathAsync(result, details, token);

            // Assert.
            result.Messages.Should()
                .HaveSameCount(expectedMessages);
            result.Messages.Should()
                .Equal(
                    expectedMessages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public void TestBackupDatabaseWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestBackupDatabaseWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            // Act.
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            List<string> expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = _sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Messages.Should()
                .HaveSameCount(expectedMessages);
            result.Messages.Should()
                .Equal(
                    expectedMessages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            CancellationToken token = new CancellationToken();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            _command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new OperationResult();

            List<string> expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = await _sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

            // Assert.
            result.Messages.Should()
                .HaveSameCount(expectedMessages);
            result.Messages.Should()
                .Equal(
                    expectedMessages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        private static ConnectionOptions GetConnectionOptions(
            string serverParameter,
            string databaseParameter,
            string serverName,
            string databaseName)
        {
            string connectionString =
                $"{serverParameter}={serverName};{databaseParameter}={databaseName};User Id=sa;Password=password;Connect Timeout=10;";
            return new ConnectionOptions(connectionString, BackupPath);
        }
    }
}