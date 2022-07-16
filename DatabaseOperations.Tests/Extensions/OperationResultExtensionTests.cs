namespace DatabaseOperations.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Extensions;
    using FluentAssertions;
    using Interfaces;
    using NSubstitute;
    using Xunit;

    public class OperationResultExtensionTests
    {
        public OperationResultExtensionTests()
        {
            _tokenSource = new CancellationTokenSource();
            _connectionOptions = new ConnectionOptions("something", @"C:\Database Backups\", 5)
                .ApplyServer("127.0.0.1")
                .ApplyDatabaseName("Bananas")
                .ApplyUserId("sa")
                .ApplyPassword("password")
                .ApplyConnectTimeOut("205");
            _sqlExecutor = Substitute.For<ISqlExecutor>();
        }

        private readonly ConnectionOptions _connectionOptions;
        private readonly ISqlExecutor _sqlExecutor;

        private readonly CancellationTokenSource _tokenSource;

        [Fact]
        public void TestValidateOptionsWithValidOptionsReturnsExpectedResult()
        {
            // Arrange.
            OperationResult expected = new OperationResult();

            // Act.
            OperationResult actual = expected.ValidateConnectionOptions(_connectionOptions);

            // Assert.
            actual.Messages.Should()
                .BeEmpty();
            actual.Result.Should()
                .BeTrue();
        }

        [Fact]
        public void TestValidateOptionsWithInvalidOptionsReturnsExpectedResult()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _connectionOptions.ApplyServer(string.Empty);

            // Act.
            OperationResult actual = expected.ValidateConnectionOptions(_connectionOptions);

            // Assert.
            actual.Messages.Count.Should()
                .Be(1);
            actual.Result.Should()
                .BeFalse();
        }

        [Fact]
        public void TestExecuteBackupPathWithFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };

            // Act.
            OperationResult actual = expected.ExecuteBackupPath(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            _sqlExecutor.Received(0)
                .ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestExecuteBackupPathWithTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(expected);

            // Act.
            OperationResult actual = expected.ExecuteBackupPath(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            _sqlExecutor.Received(1)
                .ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestCheckBackupPathWithMessagesResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = true,
                Messages = new List<string>
                {
                    "Backup path folder check/create failed due to an exception.",
                    "Unable to check the path, reverting to default save path."
                }
            };
            OperationResult input = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
            };

            // Act.
            OperationResult actual = input.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Messages.Should()
                .HaveSameCount(expected.Messages);
            actual.Messages.Should()
                .Equal(
                    expected.Messages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public void TestCheckBackupPathWithNoMessagesAndFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false
            };

            // Act.
            OperationResult actual = expected.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public void TestCheckBackupPathWithNoMessagesAndTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = true
            };

            // Act.
            OperationResult actual = expected.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public void TestExecuteBackupWithFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };

            // Act.
            OperationResult actual = expected.ExecuteBackup(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            _sqlExecutor.Received(0)
                .ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestExecuteBackupWithTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>())
                .Returns(expected);

            // Act.
            OperationResult actual = expected.ExecuteBackup(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            _sqlExecutor.Received(1)
                .ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public async Task TestValidateOptionsAsyncWithValidOptionsReturnsExpectedResult()
        {
            // Arrange.
            OperationResult expected = new OperationResult();

            // Act.
            OperationResult actual = await expected.ValidateConnectionOptionsAsync(_connectionOptions)
                .ConfigureAwait(false);

            // Assert.
            actual.Messages.Should()
                .BeEmpty();
            actual.Result.Should()
                .BeTrue();
        }

        [Fact]
        public async Task TestValidateOptionsAsyncWithInvalidOptionsReturnsExpectedResult()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _connectionOptions.ApplyServer(string.Empty);

            // Act.
            OperationResult actual = await expected.ValidateConnectionOptionsAsync(_connectionOptions)
                .ConfigureAwait(false);

            // Assert.
            actual.Messages.Count.Should()
                .Be(1);
            actual.Result.Should()
                .BeFalse();
        }

        [Fact]
        public async Task TestCheckForCancellationWithFalseReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .CheckForCancellation(token)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckForCancellationWithTrueForFirstCheckReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Cancel called on the token." }
            };
            OperationResult input = new OperationResult();
            CancellationToken token = _tokenSource.Token;
            _tokenSource.Cancel();

            // Act.
            OperationResult actual = await Task.FromResult(input)
                .CheckForCancellation(token)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckForCancellationWithFalseForOtherChecksReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Cancel called on the token." }
            };
            CancellationToken token = _tokenSource.Token;
            _tokenSource.Cancel();

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .CheckForCancellation(token)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestExecuteBackupPathAsyncWithFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .ExecuteBackupPathAsync(_connectionOptions, token, _sqlExecutor)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            await _sqlExecutor.Received(0)
                .ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExecuteBackupPathAsyncWithTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _sqlExecutor.ExecuteBackupPathAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(expected);
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .ExecuteBackupPathAsync(_connectionOptions, token, _sqlExecutor)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            await _sqlExecutor.Received(1)
                .ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>())
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithMessagesResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = true,
                Messages = new List<string>
                {
                    "Backup path folder check/create failed due to an exception.",
                    "Unable to check the path, reverting to default save path."
                }
            };
            OperationResult input = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
            };
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(input)
                .CheckBackupPathExecutionAsync(_connectionOptions, token)
                .ConfigureAwait(false);

            // Assert.
            actual.Messages.Should()
                .HaveSameCount(expected.Messages);
            actual.Messages.Should()
                .Equal(
                    expected.Messages,
                    (
                        actualMessage,
                        expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithNoMessagesAndFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false
            };
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .CheckBackupPathExecutionAsync(_connectionOptions, token)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithNoMessagesAndTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = true
            };
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .CheckBackupPathExecutionAsync(_connectionOptions, token)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestExecuteBackupAsyncWithFalseResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .ExecuteBackupAsync(_connectionOptions, token, _sqlExecutor)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            await _sqlExecutor.Received(0)
                .ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExecuteBackupAsyncWithTrueResultReturnsExpected()
        {
            // Arrange.
            OperationResult expected = new OperationResult();
            _sqlExecutor.ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(expected);
            CancellationToken token = _tokenSource.Token;

            // Act.
            OperationResult actual = await Task.FromResult(expected)
                .ExecuteBackupAsync(_connectionOptions, token, _sqlExecutor)
                .ConfigureAwait(false);

            // Assert.
            actual.Should()
                .BeEquivalentTo(expected);
            await _sqlExecutor.Received(1)
                .ExecuteBackupDatabaseAsync(
                    Arg.Any<OperationResult>(),
                    Arg.Any<ConnectionOptions>(),
                    Arg.Any<CancellationToken>())
                .ConfigureAwait(false);
        }
    }
}