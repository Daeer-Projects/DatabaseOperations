using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;
using DatabaseOperations.Operators;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Operators
{
    public class BackupOperatorTests
    {
        public BackupOperatorTests()
        {
            _sqlExecutor = Substitute.For<ISqlExecutor>();
            _backupOperator = new BackupOperator(_sqlExecutor);
        }

        private const string BackupPath = @"C:\Database Backups\";
        private readonly ISqlExecutor _sqlExecutor;
        private readonly IBackupOperator _backupOperator;

        [Fact]
        public void TestBackupWithValidDetailsReturnsTrue()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "127.0.0.1", "Thing");

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task TestBackupAsyncWithValidDetailsReturnsTrue()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "127.0.0.1", "Thing");

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void TestBackupWithPathErrorReturnsTrue()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } });
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        public async Task TestBackupAsyncWithPathErrorReturnsTrue()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } };
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = new List<string> { "Backup path folder check/create failed due to an exception." } });
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should().BeTrue("we are reverting to the default location when backing up the database.");
        }

        [Fact]
        public void TestBackupWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            var defaultResult = new OperationResult { Result = true, Messages = expectedMessages };
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupAsyncWithPathErrorReturnsExpectedMessages()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception.",
                "Unable to check the path, reverting to default save path."
            };

            var defaultResult = new OperationResult { Result = true, Messages = expectedMessages };
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public void TestBackupWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." } });

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithDatabaseErrorReturnsFalse()
        {
            // Arrange.
            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = new List<string> { "Backing up the database failed due to an exception." } });

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupAsyncWithDatabaseErrorReturnsExpectedMessages()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = false, Messages = expectedMessages });

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, new CancellationToken());

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupPathReturnsFalse()
        {
            // Arrange.
            var source = new CancellationTokenSource(100);
            var token = source.Token;
            token.ThrowIfCancellationRequested();

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            _sqlExecutor
                .When(e => e.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            var details = GetConnectionOptions("server", "database", "oops", "Thing");

            // Act.
            var result =  await _backupOperator.BackupDatabaseAsync(details, token);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupPathReturnsCancelledMessage()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Cancel called on the token."
            };
            var source = new CancellationTokenSource(100);
            var token = source.Token;
            token.ThrowIfCancellationRequested();

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);
            _sqlExecutor
                .When(e => e.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            var details = GetConnectionOptions("server", "database", "oops", "Thing");

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, token);

            // Assert.
            result.Messages.Should().HaveSameCount(expectedMessages);
            result.Messages.Should().Equal(expectedMessages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsFalse()
        {
            // Arrange.
            var source = new CancellationTokenSource(100);
            var token = source.Token;
            token.ThrowIfCancellationRequested();

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = true });
            _sqlExecutor
                .When(e => e.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, token);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task TestBackupAsyncWithCancelledBackupDatabaseReturnsCancelledMessage()
        {
            // Arrange.
            var expectedMessages = new List<string>
            {
                "Cancel called on the token."
            };
            var source = new CancellationTokenSource(100);
            var token = source.Token;
            token.ThrowIfCancellationRequested();

            var defaultResult = new OperationResult { Result = true };
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(defaultResult);

            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .Returns(new OperationResult { Result = true });
            _sqlExecutor
                .When(e => e.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()))
                .Do(_ => Thread.Sleep(500));

            // Act.
            var result = await _backupOperator.BackupDatabaseAsync(details, token);

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
