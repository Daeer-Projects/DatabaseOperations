namespace DatabaseOperations.Tests.Operators
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Operators;
    using FluentAssertions;
    using Interfaces;
    using NSubstitute;
    using Xunit;

    public class BackupOperatorTests
    {
        public BackupOperatorTests()
        {
            sqlExecutor = Substitute.For<ISqlExecutor>();
            backupOperator = new BackupOperator(sqlExecutor);
        }

        private const string BackupPath = @"C:\Database Backups\";
        private readonly IBackupOperator backupOperator;
        private readonly ISqlExecutor sqlExecutor;

        [Fact]
        public void TestBackupWithValidDetailsReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");

            // Act.
            OperationResult result = backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestBackupAsyncWithValidDetailsReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "127.0.0.1",
                "Thing");

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestBackupWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new() { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(
                    new OperationResult
                    {
                        Result = false,
                        Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
                    });
            sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            // Act.
            OperationResult result = backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should()
                .BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        public async Task TestBackupAsyncWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new() { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(
                    new OperationResult
                    {
                        Result = false,
                        Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
                    });
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        public void TestBackupWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            OperationResult defaultResult = new() { Result = true, Messages = expectedMessages };
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);

            // Act.
            OperationResult result = backupOperator.BackupDatabase(details);

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
        public async Task TestBackupAsyncWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            OperationResult defaultResult = new() { Result = true, Messages = expectedMessages };
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, new CancellationToken());

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
        public void TestBackupWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(
                    new OperationResult
                    {
                        Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." }
                    });

            // Act.
            OperationResult result = backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(
                    new OperationResult
                    {
                        Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." }
                    });

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestBackupWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });

            // Act.
            OperationResult result = backupOperator.BackupDatabase(details);

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
        public async Task TestBackupAsyncWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, new CancellationToken());

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
        public async Task TestBackupAsyncWithCancelledBackupPathReturnsFalse()
        {
            // Arrange.
            CancellationTokenSource source = new(100);
            CancellationToken token = source.Token;
            token.ThrowIfCancellationRequested();

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor
                .When(
                    e => e.ExecuteBackupPathAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionOptions>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupPathReturnsCancelledMessage()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Cancel called on the token."
            };
            CancellationTokenSource source = new(100);
            CancellationToken token = source.Token;
            token.ThrowIfCancellationRequested();

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor
                .When(
                    e => e.ExecuteBackupPathAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionOptions>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, token);

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
        public async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsFalse()
        {
            // Arrange.
            CancellationTokenSource source = new(100);
            CancellationToken token = source.Token;
            token.ThrowIfCancellationRequested();

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = true });
            sqlExecutor
                .When(
                    e => e.ExecuteBackupDatabaseAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionOptions>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsCancelledMessage()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Cancel called on the token."
            };
            CancellationTokenSource source = new(100);
            CancellationToken token = source.Token;
            token.ThrowIfCancellationRequested();

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            ConnectionOptions details = GetConnectionOptions(
                "server",
                "database",
                "oops",
                "Thing");
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = true });
            sqlExecutor
                .When(
                    e => e.ExecuteBackupPathAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionOptions>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(details, token);

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