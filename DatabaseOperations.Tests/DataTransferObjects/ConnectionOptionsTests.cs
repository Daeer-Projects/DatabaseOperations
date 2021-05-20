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
        internal void TestConstructorWithConnectionStringReturnsExpectedApplicationName(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.ApplicationName.Should().Be(expected.ApplicationName);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedConnectionTimeout(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.ConnectTimeout.Should().Be(expected.ConnectTimeout);
        }

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
        internal void TestConstructorWithConnectionStringReturnsExpectedIntegratedSecurity(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.IntegratedSecurity.Should().Be(expected.IntegratedSecurity);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedPassword(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.Password.Should().Be(expected.Password);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedServer(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.Server.Should().Be(expected.Server);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedUser(string connectionString, int commandTimeout,
	        ConnectionOptions expected)
        {
	        // Arrange.
	        // Act.
	        var actual = new ConnectionOptions(connectionString, BackupPath, commandTimeout);

	        // Assert.
	        actual.UserId.Should().Be(expected.UserId);
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

				var actualLocation = RemoveLastSecondFromLocation(actualParameters[i].Value.ToString()!);
				var expectedLocation = RemoveLastSecondFromLocation(expectedParameters[i].Value.ToString()!);

				actualLocation.Should().Be(expectedLocation);
			}
		}

		public static IEnumerable<object[]> ConnectionStrings()
		{
			yield return new object[]
			{
				"server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=205;",
                30,
                new ConnectionOptions("database=Bananas;", BackupPath, 30)
	                .ApplyServer("127.0.0.1")
	                .ApplyDatabaseName("Bananas")
	                .ApplyUserId("sa")
	                .ApplyPassword("password")
	                .ApplyConnectTimeOut("205")
	                .OverrideConnectionString("server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=5;")
			};
			yield return new object[]
			{
				"Server=192.168.11.65;Database=Whoop;User Id=sa;Password=password;Connect Timeout=1;",
                10,
                new ConnectionOptions("database=Whoop;", BackupPath, 10)
	                .ApplyServer("192.168.11.65")
	                .ApplyDatabaseName("Whoop")
	                .ApplyUserId("sa")
	                .ApplyPassword("password")
	                .ApplyConnectTimeOut("1")
	                .OverrideConnectionString("Server=192.168.11.65;Database=Whoop;User Id=sa;Password=password;Connect Timeout=5;")
			};
			yield return new object[]
			{
				"SERVER=(localDb);DATABASE=PoohBear;User Id=sa;Password=password;Connect Timeout=30;",
                0,
                new ConnectionOptions("database=PoohBear;", BackupPath, 60 * 60)
	                .ApplyServer("(localDb)")
	                .ApplyDatabaseName("PoohBear")
	                .ApplyUserId("sa")
	                .ApplyPassword("password")
	                .ApplyConnectTimeOut("30")
	                .OverrideConnectionString("SERVER=(localDb);DATABASE=PoohBear;User Id=sa;Password=password;Connect Timeout=5;")
			};
			yield return new object[]
			{
				"Server=127.0.0.1;Address=127.0.0.1;Initial Catalog=Banana;User Id=Pooh;Password=password;Pwd=password;Application Name=TestingStuff;",
				30,
				new ConnectionOptions("database=Banana;", BackupPath, 30)
					.ApplyServer("127.0.0.1")
					.ApplyDatabaseName("Banana")
					.ApplyUserId("Pooh")
					.ApplyPassword("password")
					.ApplyApplicationName("TestingStuff")
					.OverrideConnectionString("Server=127.0.0.1;Address=127.0.0.1;Initial Catalog=Banana;User Id=Pooh;Password=password;Pwd=password;Application Name=TestingStuff;")
			};
			yield return new object[]
			{
				"Server=127.0.0.1;Address=127.0.0.1;Database=Banana;Integrated Security=true;Trusted_Connection=true;Connect Timeout=5;Connection Timeout=5;Application Name=TestingStuff;Um=42;",
				4,
				new ConnectionOptions("database=Banana;", BackupPath, 4)
					.ApplyServer("127.0.0.1")
					.ApplyDatabaseName("Banana")
					.ApplyConnectTimeOut("5")
					.ApplyIntegratedSecurity("true")
					.ApplyApplicationName("TestingStuff")
					.OverrideConnectionString("Server=127.0.0.1;Address=127.0.0.1;Database=Banana;Integrated Security=true;Trusted_Connection=true;Connect Timeout=5;Connection Timeout=5;Application Name=TestingStuff;Um=42;")
			};
			yield return new object[]
			{
				"Server=127.0.0.1;Um=42;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;",
				8,
				new ConnectionOptions("database=Banana;", BackupPath, 8)
					.ApplyServer("127.0.0.1")
					.ApplyDatabaseName("Banana")
					.ApplyConnectTimeOut("5")
					.ApplyIntegratedSecurity("true")
					.ApplyApplicationName("TestingStuff")
					.OverrideConnectionString("Server=127.0.0.1;Um=42;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;")
			};
			yield return new object[]
			{
				"Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;",
				15,
				new ConnectionOptions("database=Banana;", BackupPath, 15)
					.ApplyServer("127.0.0.1")
					.ApplyDatabaseName("Banana")
					.ApplyConnectTimeOut("5")
					.ApplyIntegratedSecurity("true")
					.ApplyApplicationName("TestingStuff")
					.OverrideConnectionString("Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;")
			};
			yield return new object[]
			{
				"Server=127.0.0.1;Data Source=127.0.0.1;addr=192.168.0.1;Initial Catalog=Banana;User Id=Pooh;Password=password;Pwd=nope;Application Name=TestingStuff;",
				25,
				new ConnectionOptions("database=Banana;", BackupPath, 25)
					.ApplyServer("127.0.0.1")
					.ApplyDatabaseName("Banana")
					.ApplyUserId("Pooh")
					.ApplyPassword("password")
					.ApplyApplicationName("TestingStuff")
					.OverrideConnectionString("Server=127.0.0.1;Data Source=127.0.0.1;addr=192.168.0.1;Initial Catalog=Banana;User Id=Pooh;Password=password;Pwd=nope;Application Name=TestingStuff;")
			};
			yield return new object[]
			{
				"; ;    ;;  ;Yep;Um not really;Pooh woz ere;",
				25,
				new ConnectionOptions(string.Empty, BackupPath, 25)
					.OverrideConnectionString("; ;    ;;  ;Yep;Um not really;Pooh woz ere;")
			};
		}

		private static string RemoveLastSecondFromLocation(string location)
		{
			return location[..^5];
		}
	}
}
