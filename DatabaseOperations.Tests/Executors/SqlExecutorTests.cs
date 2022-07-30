namespace DatabaseOperations.Tests.Executors
{
    using System.Collections.Generic;
    using System.Data;
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
            connection = Substitute.For<ISqlConnectionWrapper>();
            command = Substitute.For<ISqlCommandWrapper>();

            creator.CreateConnection(Arg.Any<string>())
                .Returns(connection);
            creator.CreateCommand(Arg.Any<string>(), connection)
                .Returns(command);
            sqlExecutor = new SqlExecutor(creator);
        }

        private const string BackupPath = @"C:\Database Backups\";
        private readonly ISqlCommandWrapper command;
        private readonly ISqlConnectionWrapper connection;
        private readonly ISqlExecutor sqlExecutor;

        [Fact]
        public void TestBackupPathWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestBackupPathActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, connProps, backup);

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
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestBackupPathAsyncActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(
                result,
                connProps,
                backup,
                new CancellationToken());

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
            connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathActionWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, connProps, backup);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncActionWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(
                result,
                connProps,
                backup,
                token);

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
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathActionWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, connProps, backup);

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
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncActionWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(
                result,
                connProps,
                backup,
                new CancellationToken());

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
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupPathActionWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, connProps, backup);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupPathAsyncActionWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(
                result,
                connProps,
                backup,
                token);

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
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, details);

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
        public void TestBackupPathActionWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = sqlExecutor.ExecuteBackupPath(result, connProps, backup);

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
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(result, details, token);

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
        public async Task TestBackupPathAsyncActionWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backup path folder check/create failed due to an exception."
            };

            // Act.
            result = await sqlExecutor.ExecuteBackupPathAsync(
                result,
                connProps,
                backup,
                token);

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
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestBackupDatabaseActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, connProps, backup);

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
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(
                result,
                connProps,
                backup,
                new CancellationToken());

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
            connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseActionWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            connection
                .When(c => c.Open())
                .Do(_ => throw new DbTestException("Server is not correct!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, connProps, backup);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncActionWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            connection
                .When(c => c.OpenAsync(token))
                .Do(_ => throw new DbTestException("Server is not correct!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(
                result,
                connProps,
                backup,
                token);

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
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseActionWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, connProps, backup);

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
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(result, details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncActionWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(
                result,
                connProps,
                backup,
                new CancellationToken());

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
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupDatabaseActionWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, connProps, backup);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupDatabaseAsyncActionWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(
                result,
                connProps,
                backup,
                token);

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
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, details);

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
        public void TestBackupDatabaseActionWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = sqlExecutor.ExecuteBackupDatabase(result, connProps, backup);

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
            CancellationToken token = new();
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));
            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(result, details, token);

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
        public async Task TestBackupDatabaseAsyncActionWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            CancellationToken token = new();
            ConnectionProperties connProps = GetValidConnectionProperties();
            BackupProperties backup = GetValidBackupProperties();
            command
                .When(c => c.ExecuteNonQueryAsync(token))
                .Do(_ => throw new DbTestException("Execute is not working!"));

            OperationResult result = new();

            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            result = await sqlExecutor.ExecuteBackupDatabaseAsync(
                result,
                connProps,
                backup,
                token);

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

        private static BackupProperties GetValidBackupProperties()
        {
            SqlParameter dataParam = new() { ParameterName = "@Database", DbType = DbType.String };
            BackupProperties backup = new()
            {
                BackupFileName = "BackupFile.bak",
                BackupPath = BackupPath,
                BackupParameters = new[] { dataParam },
                CommandTimeout = 5,
                Description = "Some backup",
                ExecutionParameters = new[] { dataParam }
            };
            return backup;
        }

        private static ConnectionProperties GetValidConnectionProperties()
        {
            ConnectionProperties connProps = new()
                { Server = "server", DatabaseName = "database", IntegratedSecurity = "True", ConnectTimeout = "5" };
            return connProps;
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