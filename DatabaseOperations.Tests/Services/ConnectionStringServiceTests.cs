namespace DatabaseOperations.Tests.Services
{
    using System.Collections.Generic;
    using DatabaseOperations.DataTransferObjects;
    using DatabaseOperations.Services;
    using FluentAssertions;
    using Xunit;

    public class ConnectionStringServiceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        internal void TestExtractWithEmptyConnectionStringReturnsNewProperties(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.Should()
                .BeOfType<ConnectionProperties>()
                .Subject.Should()
                .NotBeNull();
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedServer(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.Server.Should()
                .Be("127.0.0.1");
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedDatabase(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.DatabaseName.Should()
                .Be("Bananas");
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedAndChangedTimeout(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.ConnectTimeout.Should()
                .Be("205");
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedApplicationName(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.ApplicationName.Should()
                .Be("Test App");
        }

        [Theory]
        [MemberData(nameof(ConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedConnectionString(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.ConnectionString.Should()
                .NotBeNullOrWhiteSpace();
        }

        [Theory]
        [MemberData(nameof(UserConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedUserId(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.UserId.Should()
                .Be("sa");
        }

        [Theory]
        [MemberData(nameof(UserConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedPassword(string connection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.Password.Should()
                .Be("p@55word1");
        }

        [Theory]
        [MemberData(nameof(TrustedConnectionStrings))]
        internal void TestExtractWithValidDataReturnsExpectedSecurity(
            string connection,
            string expectedConnection)
        {
            // Arrange.
            // Act.
            ConnectionProperties actual = ConnectionStringService.ExtractConnectionParameters(connection);

            // Assert.
            actual.IntegratedSecurity.Should()
                .Be(expectedConnection);
        }

        internal static IEnumerable<object[]> ConnectionStrings()
        {
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "data source=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "address=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "addr=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "network address=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connection Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;initial catalog=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=True;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=False;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=SSPI;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=True;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=False;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=sspi;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;pwd=p@55word1;Connect Timeout=205;application name=Test App;"
            };
        }

        internal static IEnumerable<object[]> UserConnectionStrings()
        {
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "data source=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "address=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "addr=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "network address=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;Password=p@55word1;Connection Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;initial catalog=Bananas;User Id=sa;Password=p@55word1;Connect Timeout=205;application name=Test App;"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;User Id=sa;pwd=p@55word1;Connect Timeout=205;application name=Test App;"
            };
        }

        internal static IEnumerable<object[]> TrustedConnectionStrings()
        {
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=True;Connect Timeout=205;application name=Test App;",
                "True"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=False;Connect Timeout=205;application name=Test App;",
                "False"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Integrated Security=SSPI;Connect Timeout=205;application name=Test App;",
                "SSPI"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=True;Connect Timeout=205;application name=Test App;", "True"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=False;Connect Timeout=205;application name=Test App;",
                "False"
            };
            yield return new object[]
            {
                "server=127.0.0.1;database=Bananas;Trusted_Connection=sspi;Connect Timeout=205;application name=Test App;", "sspi"
            };
        }
    }
}