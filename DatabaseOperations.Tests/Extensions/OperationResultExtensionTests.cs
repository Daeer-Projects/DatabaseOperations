using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Extensions
{
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

        private readonly CancellationTokenSource _tokenSource;
        private readonly ConnectionOptions _connectionOptions;
        private readonly ISqlExecutor _sqlExecutor;

        [Fact]
        public void TestValidateOptionsWithValidOptionsReturnsExpectedResult()
        {
            // Arrange.
            var expected = new OperationResult();

            // Act.
            var actual = expected.ValidateConnectionOptions(_connectionOptions);

            // Assert.
            actual.Messages.Should().BeEmpty();
            actual.Result.Should().BeTrue();
        }

        [Fact]
        public void TestValidateOptionsWithInvalidOptionsReturnsExpectedResult()
        {
            // Arrange.
            var expected = new OperationResult();
            _connectionOptions.ApplyServer(string.Empty);

            // Act.
            var actual = expected.ValidateConnectionOptions(_connectionOptions);

            // Assert.
            actual.Messages.Count.Should().Be(1);
            actual.Result.Should().BeFalse();
        }

        [Fact]
        public void TestExecuteBackupPathWithFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };

            // Act.
            var actual = expected.ExecuteBackupPath(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            _sqlExecutor.Received(0).ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestExecuteBackupPathWithTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult();
            _sqlExecutor.ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>()).Returns(expected);

            // Act.
            var actual = expected.ExecuteBackupPath(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            _sqlExecutor.Received(1).ExecuteBackupPath(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestCheckBackupPathWithMessagesResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = true,
                Messages = new List<string>
                {
                    "Backup path folder check/create failed due to an exception.",
                    "Unable to check the path, reverting to default save path."
                }
            };
            var input = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
            };

            // Act.
            var actual = input.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Messages.Should().HaveSameCount(expected.Messages);
            actual.Messages.Should().Equal(expected.Messages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public void TestCheckBackupPathWithNoMessagesAndFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false
            };

            // Act.
            var actual = expected.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TestCheckBackupPathWithNoMessagesAndTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = true
            };

            // Act.
            var actual = expected.CheckBackupPathExecution(_connectionOptions);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TestExecuteBackupWithFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };

            // Act.
            var actual = expected.ExecuteBackup(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            _sqlExecutor.Received(0).ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public void TestExecuteBackupWithTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult();
            _sqlExecutor.ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>()).Returns(expected);

            // Act.
            var actual = expected.ExecuteBackup(_connectionOptions, _sqlExecutor);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            _sqlExecutor.Received(1).ExecuteBackupDatabase(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>());
        }

        [Fact]
        public async Task TestValidateOptionsAsyncWithValidOptionsReturnsExpectedResult()
        {
            // Arrange.
            var expected = new OperationResult();

            // Act.
            var actual = await expected.ValidateConnectionOptionsAsync(_connectionOptions).ConfigureAwait(false);

            // Assert.
            actual.Messages.Should().BeEmpty();
            actual.Result.Should().BeTrue();
        }

        [Fact]
        public async Task TestValidateOptionsAsyncWithInvalidOptionsReturnsExpectedResult()
        {
            // Arrange.
            var expected = new OperationResult();
            _connectionOptions.ApplyServer(string.Empty);

            // Act.
            var actual = await expected.ValidateConnectionOptionsAsync(_connectionOptions).ConfigureAwait(false);

            // Assert.
            actual.Messages.Count.Should().Be(1);
            actual.Result.Should().BeFalse();
        }

        [Fact]
        public async Task TestCheckForCancellationWithFalseReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult();
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).CheckForCancellation(token).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckForCancellationWithTrueForFirstCheckReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Cancel called on the token." }
            };
            var input = new OperationResult();
            var token = _tokenSource.Token;
            _tokenSource.Cancel();

            // Act.
            var actual = await Task.FromResult(input).CheckForCancellation(token).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckForCancellationWithFalseForOtherChecksReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Cancel called on the token." }
            };
            var token = _tokenSource.Token;
            _tokenSource.Cancel();

            // Act.
            var actual = await Task.FromResult(expected).CheckForCancellation(token).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestExecuteBackupPathAsyncWithFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).ExecuteBackupPathAsync(_connectionOptions, token, _sqlExecutor).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            await _sqlExecutor.Received(0).ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExecuteBackupPathAsyncWithTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult();
            _sqlExecutor.ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).Returns(expected);
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).ExecuteBackupPathAsync(_connectionOptions, token, _sqlExecutor).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            await _sqlExecutor.Received(1).ExecuteBackupPathAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithMessagesResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = true,
                Messages = new List<string>
                {
                    "Backup path folder check/create failed due to an exception.",
                    "Unable to check the path, reverting to default save path."
                }
            };
            var input = new OperationResult
            {
                Result = false,
                Messages = new List<string> { "Backup path folder check/create failed due to an exception." }
            };
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(input).CheckBackupPathExecutionAsync(_connectionOptions, token).ConfigureAwait(false);

            // Assert.
            actual.Messages.Should().HaveSameCount(expected.Messages);
            actual.Messages.Should().Equal(expected.Messages, (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithNoMessagesAndFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false
            };
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).CheckBackupPathExecutionAsync(_connectionOptions, token).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestCheckBackupPathAsyncWithNoMessagesAndTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = true
            };
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).CheckBackupPathExecutionAsync(_connectionOptions, token).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task TestExecuteBackupAsyncWithFalseResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult
            {
                Result = false,
                Messages = new List<string>()
            };
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).ExecuteBackupAsync(_connectionOptions, token, _sqlExecutor).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            await _sqlExecutor.Received(0).ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExecuteBackupAsyncWithTrueResultReturnsExpected()
        {
            // Arrange.
            var expected = new OperationResult();
            _sqlExecutor.ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).Returns(expected);
            var token = _tokenSource.Token;

            // Act.
            var actual = await Task.FromResult(expected).ExecuteBackupAsync(_connectionOptions, token, _sqlExecutor).ConfigureAwait(false);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
            await _sqlExecutor.Received(1).ExecuteBackupDatabaseAsync(Arg.Any<OperationResult>(), Arg.Any<ConnectionOptions>(), Arg.Any<CancellationToken>()).ConfigureAwait(false);
        }
    }
}
