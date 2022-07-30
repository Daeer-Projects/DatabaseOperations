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

    public sealed class BackupOperatorTests
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
        internal void TestBackupWithValidDetailsReturnsTrue()
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
        internal void TestBackupActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabase(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = backupOperator.BackupDatabase(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        internal async Task TestBackupAsyncWithValidDetailsReturnsTrue()
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
        internal async Task TestBackupAsyncActionWithValidDetailsReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeTrue();
        }

        [Fact]
        internal void TestBackupWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new()
                { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
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
        internal void TestBackupActionWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new()
                { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            sqlExecutor.ExecuteBackupPath(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(
                    new OperationResult
                    {
                        Result = false,
                        Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
                    });
            sqlExecutor.ExecuteBackupDatabase(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = backupOperator.BackupDatabase(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        internal async Task TestBackupAsyncWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new()
                { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
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
        internal async Task TestBackupAsyncActionWithPathErrorReturnsTrue()
        {
            // Arrange.
            OperationResult defaultResult = new()
                { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(
                    new OperationResult
                    {
                        Result = false,
                        Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
                    });
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        internal void TestBackupWithPathErrorReturnsExpectedMessages()
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
        internal void TestBackupActionWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            OperationResult defaultResult = new() { Result = true, Messages = expectedMessages };
            sqlExecutor.ExecuteBackupPath(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(
                    new OperationResult
                    {
                        Result = false,
                        Messages = expectedMessages
                    });
            sqlExecutor.ExecuteBackupDatabase(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = backupOperator.BackupDatabase(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

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
        internal async Task TestBackupAsyncWithPathErrorReturnsExpectedMessages()
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
        internal async Task TestBackupAsyncActionWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            OperationResult defaultResult = new() { Result = true, Messages = expectedMessages };
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

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
        internal void TestBackupWithDatabaseErrorReturnsFalse()
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
        internal void TestBackupActionWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabase(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(
                    new OperationResult
                    {
                        Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." }
                    });
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = backupOperator.BackupDatabase(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        internal async Task TestBackupAsyncWithDatabaseErrorReturnsFalse()
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
        internal async Task TestBackupAsyncActionWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(
                    new OperationResult
                    {
                        Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." }
                    });
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        internal void TestBackupWithDatabaseErrorReturnsExpectedMessages()
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
        internal void TestBackupActionWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPath(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabase(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = backupOperator.BackupDatabase(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

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
        internal async Task TestBackupAsyncWithDatabaseErrorReturnsExpectedMessages()
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
        internal async Task TestBackupAsyncActionWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            List<string> expectedMessages = new()
            {
                "Backing up the database failed due to an exception."
            };

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            const string connectionString = "server=server;database=database;user id=user;password=password";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                });

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
        internal async Task TestBackupAsyncWithCancelledBackupPathReturnsFalse()
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
        internal async Task TestBackupAsyncActionWithCancelledBackupPathReturnsFalse()
        {
            // Arrange.
            CancellationTokenSource source = new(100);
            CancellationToken token = source.Token;
            token.ThrowIfCancellationRequested();

            OperationResult defaultResult = new();
            sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor
                .When(
                    e => e.ExecuteBackupPathAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionProperties>(),
                        Arg.Any<BackupProperties>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));
            const string connectionString = "server=server;database=database;user id=user;password=password;Connect Timeout=10;";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                },
                token);

            // Assert.
            result.Result.Should()
                .BeFalse();
        }

        [Fact]
        internal async Task TestBackupAsyncWithCancelledBackupPathReturnsCancelledMessage()
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
        internal async Task TestBackupAsyncActionWithCancelledBackupPathReturnsCancelledMessage()
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
                    Arg.Any<ConnectionProperties>(),
                    Arg.Any<BackupProperties>(),
                    Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            sqlExecutor
                .When(
                    e => e.ExecuteBackupPathAsync(
                        Arg.Any<OperationResult>(),
                        Arg.Any<ConnectionProperties>(),
                        Arg.Any<BackupProperties>(),
                        Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));
            const string connectionString = "server=server;database=database;user id=user;password=password;Connect Timeout=10;";

            // Act.
            OperationResult result = await backupOperator.BackupDatabaseAsync(
                connectionString,
                options =>
                {
                    options.BackupPath = BackupPath;
                    options.Timeout = 100;
                },
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
        internal async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsFalse()
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
        internal async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsCancelledMessage()
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