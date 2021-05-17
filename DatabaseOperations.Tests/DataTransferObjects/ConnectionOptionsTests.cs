using System.Collections.Generic;
using DatabaseOperations.DataTransferObjects;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.DataTransferObjects
{

	public class ConnectionOptionsTests
	{
        private const string BackupPath = @"C:\Database Backups\";

		[Theory]
		[MemberData(nameof(ConnectionStrings))]
		internal void TestConstructorWithConnectionStringReturnsExpectedDatabaseName(string connectionString, int commandTimeout,
			ConnectionOptions expected)
		{
			// Arrange.
			// Act.
			var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

			// Assert.
			actual.DatabaseName.Should().Be(expected.DatabaseName);
		}

		[Theory]
		[MemberData(nameof(ConnectionStrings))]
		internal void TestConstructorWithConnectionStringReturnsExpectedConnectionString(string connectionString, int commandTimeout,
			ConnectionOptions expected)
		{
			// Arrange.
			// Act.
            var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

			// Assert.
			actual.ConnectionString.Should().Be(expected.ConnectionString);
		}

		[Theory]
		[MemberData(nameof(ConnectionStrings))]
		internal void TestConstructorWithConnectionStringReturnsExpectedBackupLocation(string connectionString, int commandTimeout,
			ConnectionOptions expected)
		{
			// Arrange.
			// Act.
            var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);
			var actualLocation = RemoveLastSecondFromLocation(actual.BackupLocation);
			var expectedLocation = RemoveLastSecondFromLocation(expected.BackupLocation);

			// Assert.
			actualLocation.Should().Be(expectedLocation);
		}

		[Theory]
		[MemberData(nameof(ConnectionStrings))]
		internal void TestConstructorWithConnectionStringReturnsExpectedDescription(string connectionString, int commandTimeout,
			ConnectionOptions expected)
		{
			// Arrange.
			// Act.
            var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

			// Assert.
			actual.Description.Should().Be(expected.Description);
		}

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedCommandTimeout(string connectionString, int commandTimeout,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

            // Assert.
            actual.CommandTimeout.Should().Be(expected.CommandTimeout);
        }

		[Theory]
		[MemberData(nameof(ConnectionStrings))]
		internal void TestConstructorWithConnectionStringReturnsExpectedParameters(string connectionString, int commandTimeout,
			ConnectionOptions expected)
		{
			// Arrange.
            var expectedParameters = expected.Parameters();

			// Act.
            var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);
            var actualParameters = actual.Parameters();

			// Assert.
			for (var i = 0; i < actualParameters.Length; i++)
			{
				if (actualParameters[i].ParameterName != Constants.Parameters.LocationParameter)
				{
                    actualParameters[i].Value.Should().BeEquivalentTo(expectedParameters[i].Value);
					continue;
				}

				var actualLocation = RemoveLastSecondFromLocation(actualParameters[i].Value.ToString());
				var expectedLocation = RemoveLastSecondFromLocation(expectedParameters[i].Value.ToString());

				actualLocation.Should().Be(expectedLocation);
			}
		}

		public static IEnumerable<object[]> ConnectionStrings()
		{
			yield return new object[]
			{
				"server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=205;",
                30,
				GetConnectionOptions("server", "database", "127.0.0.1", "Bananas", 5, 30)
			};
			yield return new object[]
			{
				"Server=192.168.11.65;Database=Whoop;User Id=sa;Password=password;Connect Timeout=1;",
                10,
				GetConnectionOptions("Server", "Database", "192.168.11.65", "Whoop", 5, 10)
			};
			yield return new object[]
			{
				"SERVER=(localDb);DATABASE=PoohBear;User Id=sa;Password=password;Connect Timeout=30;",
                0,
				GetConnectionOptions("SERVER", "DATABASE", "(localDb)", "PoohBear", 5, 60 * 60)
			};
		}

		private static ConnectionOptions GetConnectionOptions(string serverParameter, string databaseParameter, string serverName, string databaseName, int timeout, int commandTimeout)
		{
			var connectionString = $"{serverParameter}={serverName};{databaseParameter}={databaseName};User Id=sa;Password=password;Connect Timeout={timeout};";
			return new ConnectionOptions(connectionString, BackupPath, commandTimeout);
		}

		private static string RemoveLastSecondFromLocation(string location)
		{
			return location[..^5];
		}
	}
}
