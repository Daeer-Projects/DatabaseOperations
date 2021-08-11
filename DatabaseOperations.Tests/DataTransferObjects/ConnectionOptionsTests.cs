using System;
using System.Collections.Generic;
using DatabaseOperations.DataTransferObjects;
using DatabaseOperations.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.DataTransferObjects
{
    public class ConnectionOptionsTests
    {
        public ConnectionOptionsTests()
        {
            DateTimeWrapper.Now.Returns(new DateTime(2021, 08, 11, 15, 52, 08));
        }

        private static readonly IDateTimeWrapper DateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        private const string BackupPath = @"C:\Database Backups\";

        [Fact]
        public void TestConstructorWithEmptyConnectionStringReturnsValidationFailed()
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(string.Empty, DateTimeWrapper, BackupPath);

            // Assert.
            actual.IsValid().Should().BeFalse();
        }

        [Fact]
        public void TestConstructorWithNullConnectionStringReturnsValidationFailed()
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(null!, DateTimeWrapper, BackupPath);

            // Assert.
            actual.IsValid().Should().BeFalse();
        }

        [Fact]
        internal void TestConstructorWithConnectionStringReturnsIsValid()
        {
            // Arrange.
            const string connectionString = "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=205;";
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper);

            // Assert.
            actual.IsValid().Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedApplicationName(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.ApplicationName.Should().Be(expected.ApplicationName);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedConnectionTimeout(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.ConnectTimeout.Should().Be(expected.ConnectTimeout);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedDatabaseName(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.DatabaseName.Should().Be(expected.DatabaseName);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedConnectionString(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.ConnectionString.Should().Be(expected.ConnectionString);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedBackupLocation(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedDescription(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.Description.Should().Be(expected.Description);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedCommandTimeout(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.CommandTimeout.Should().Be(expected.CommandTimeout);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedIntegratedSecurity(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.IntegratedSecurity.Should().Be(expected.IntegratedSecurity);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedPassword(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.Password.Should().Be(expected.Password);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedServer(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.Server.Should().Be(expected.Server);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedUser(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);

            // Assert.
            actual.UserId.Should().Be(expected.UserId);
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedBackupParameters(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            var expectedParameters = expected.BackupParameters();

            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);
            var actualParameters = actual.BackupParameters();

            // Assert.
            for (var i = 0; i < actualParameters.Length; i++)
            {
                var actualLocation = actualParameters[i].Value.ToString()!;
                var expectedLocation = expectedParameters[i].Value.ToString()!;

                actualLocation.Should().Be(expectedLocation);
            }
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestConstructorWithConnectionStringReturnsExpectedExecutionParameters(string connectionString, int commandTimeout, string backupPath,
            ConnectionOptions expected)
        {
            // Arrange.
            var expectedParameters = expected.ExecutionParameters();

            // Act.
            var actual = new ConnectionOptions(connectionString, DateTimeWrapper, backupPath, commandTimeout);
            var actualParameters = actual.ExecutionParameters();

            // Assert.
            for (var i = 0; i < actualParameters.Length; i++)
            {
                actualParameters[i].Value.Should().BeEquivalentTo(expectedParameters[i].Value);
            }
        }

        [Fact]
        internal void TestRemovePathReturnsPathIsNotTheSameAsBefore()
        {
            // Arrange.
            var options =
                new ConnectionOptions(
                    "SERVER=(localDb);DATABASE=PoohBear;User Id=sa;Password=password;Connect Timeout=30;", DateTimeWrapper,
                    @"H:\Backups\");
            var currentBackupLocation = options.BackupLocation;


            // Act.
            options.RemovePathFromBackupLocation();

            // Assert.
            currentBackupLocation.Should().NotBe(options.BackupLocation);
        }

        [Fact]
        internal void TestRemovePathReturnsPathIsContainedWithinOldPath()
        {
            // Arrange.
            var options =
                new ConnectionOptions(
                    "SERVER=(localDb);DATABASE=PoohBear;User Id=sa;Password=password;Connect Timeout=30;", DateTimeWrapper,
                    @"H:\Backups\");
            var currentBackupLocation = options.BackupLocation;


            // Act.
            options.RemovePathFromBackupLocation();

            // Assert.
            currentBackupLocation.Should().Contain(options.BackupLocation);
        }

        public static IEnumerable<object[]> ConnectionStrings()
        {
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=205;",
                30,
                "",
                new ConnectionOptions("database=Bananas;", DateTimeWrapper, string.Empty, 30)
                    .ApplyServer("127.0.0.1")
                    .ApplyDatabaseName("Bananas")
                    .ApplyUserId("sa")
                    .ApplyPassword("password")
                    .ApplyConnectTimeOut("205")
                    .OverrideConnectionString("server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=5;")
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=205;",
                30,
                BackupPath,
                new ConnectionOptions("database=Bananas;", DateTimeWrapper, BackupPath, 30)
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
                BackupPath,
                new ConnectionOptions("database=Whoop;", DateTimeWrapper, BackupPath, 10)
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
                BackupPath,
                new ConnectionOptions("database=PoohBear;", DateTimeWrapper, BackupPath, 60 * 60)
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
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 30)
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
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 4)
                    .ApplyServer("127.0.0.1")
                    .ApplyDatabaseName("Banana")
                    .ApplyConnectTimeOut("5")
                    .ApplyIntegratedSecurity("true")
                    .ApplyApplicationName("TestingStuff")
                    .OverrideConnectionString("Server=127.0.0.1;Address=127.0.0.1;Database=Banana;Integrated Security=true;Trusted_Connection=true;Connect Timeout=5;Connection Timeout=5;Application Name=TestingStuff;Um=42;")
            };
            yield return new object[]
            {
                "Server=127.0.0.1;Um=42;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;Connect Timeout=5;Um=42;Connection Timeout=6;Application Name=TestingStuff;",
                8,
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 8)
                    .ApplyServer("127.0.0.1")
                    .ApplyDatabaseName("Banana")
                    .ApplyConnectTimeOut("5")
                    .ApplyIntegratedSecurity("true")
                    .ApplyApplicationName("TestingStuff")
                    .OverrideConnectionString("Server=127.0.0.1;Um=42;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=true;;Trusted_Connection=true;Connect Timeout=5;Um=42;Connection Timeout=6;Application Name=TestingStuff;")
            };
            yield return new object[]
            {
                "Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=false;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;",
                15,
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 15)
                    .ApplyServer("127.0.0.1")
                    .ApplyDatabaseName("Banana")
                    .ApplyConnectTimeOut("5")
                    .ApplyIntegratedSecurity("false")
                    .ApplyApplicationName("TestingStuff")
                    .OverrideConnectionString("Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=false;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;")
            };
            yield return new object[]
            {
                "Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=SSPI;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;",
                15,
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 15)
                    .ApplyServer("127.0.0.1")
                    .ApplyDatabaseName("Banana")
                    .ApplyConnectTimeOut("5")
                    .ApplyIntegratedSecurity("SSPI")
                    .ApplyApplicationName("TestingStuff")
                    .OverrideConnectionString("Server=127.0.0.1;Something weird here;Address=127.0.0.1;Database=Banana;Um=42;Integrated Security=SSPI;;Trusted_Connection=true;    ;Connect Timeout=5;Um=42;Connection Timeout=5;Application Name=TestingStuff;")
            };
            yield return new object[]
            {
                "Server=127.0.0.1;Data Source=127.0.0.1;addr=192.168.0.1;Initial Catalog=Banana;User Id=Pooh;Password=password;Pwd=nope;Application Name=TestingStuff;",
                25,
                BackupPath,
                new ConnectionOptions("database=Banana;", DateTimeWrapper, BackupPath, 25)
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
                BackupPath,
                new ConnectionOptions("Something", DateTimeWrapper, BackupPath, 25)
                    .OverrideConnectionString("; ;    ;;  ;Yep;Um not really;Pooh woz ere;")
            };
        }
    }
}
