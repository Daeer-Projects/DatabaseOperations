﻿using System.Collections.Generic;
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
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void TestBackupWithConnectionErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _connection
                .When(c => c.Open())
                .Do(_ => throw  new DbTestException("Server is not correct!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithCommandParameterErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.AddParameters(Arg.Any<SqlParameter[]>()))
                .Do(_ => throw new DbTestException("Command is not working!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithCommandExecuteErrorReturnsFalse()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            // Act.
            var result = _backupOperator.BackupDatabase(details);

            // Assert.
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void TestBackupWithCommandExecuteErrorReturnsExpectedMessages()
        {
            // Arrange.
            var details = GetConnectionOptions("server", "database", "oops", "Thing");
            _command
                .When(c => c.ExecuteNonQuery())
                .Do(_ => throw new DbTestException("Execute is not working!"));

            var expectedMessages = new List<string>
            {
                "Backing up the database failed due to an exception."
            };

            // Act.
            var result = _backupOperator.BackupDatabase(details);

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
