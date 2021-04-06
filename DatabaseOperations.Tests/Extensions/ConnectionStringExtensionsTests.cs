using DatabaseOperations.Extensions;
using DatabaseOperations.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DatabaseOperations.Tests.Extensions
{
    public class ConnectionStringExtensionsTests
	{
		[Theory]
		[InlineData("")]
		[InlineData("blah;")]
		[InlineData("blah;made;what")]
		[InlineData("server=;ah;what;no;")]
		[InlineData("server=;")]
		[InlineData("Server=;")]
		[InlineData("blah;Database=;user;password;tout")]
		[InlineData("blah;database=;user;password;tout")]
		public void TestToConnectionDetailsWithInvalidStringReturnsDefault(string connectionString)
		{
			// Arrange.
			// Act.
			var actual = connectionString.ToConnectionDetails(Substitute.For<IConsoleWrapper>());

			// Assert.
			actual.IsValid.Should().BeFalse();
		}

		[Theory]
		[InlineData("server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;")]
		[InlineData("Server=127.0.0.1;Database=Bananas;User Id=sa;Password=password;Connect Timeout=10;")]
		[InlineData("SERVER=127.0.0.1;DATABASE=Bananas;User Id=sa;Password=password;Connect Timeout=10;")]
		public void TestToConnectionDetailsWithValidStringReturnsDefault(string connectionString)
		{
			// Arrange.
			// Act.
			var actual = connectionString.ToConnectionDetails(Substitute.For<IConsoleWrapper>());

			// Assert.
			actual.IsValid.Should().BeTrue();
		}

		[Fact]
		public void TestToConnectionDetailsWithValidStringReturnsExpectedDatabase()
		{
			// Arrange.
			const string connectionString = "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;";
			const string expected = "Bananas";

			// Act.
			var actual = connectionString.ToConnectionDetails(Substitute.For<IConsoleWrapper>());

			// Assert.
			actual.DatabaseName.Should().Be(expected);
		}

		[Fact]
		public void TestToConnectionDetailsWithValidStringReturnsExpectedServer()
		{
			// Arrange.
			const string connectionString = "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;";
			const string expected = "127.0.0.1";

			// Act.
			var actual = connectionString.ToConnectionDetails(Substitute.For<IConsoleWrapper>());

			// Assert.
			actual.ServerName.Should().Be(expected);
		}

		[Fact]
		public void TestToConnectionDetailsWithValidStringReturnsExpectedConnectionString()
		{
			// Arrange.
			const string connectionString = "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;";

			// Act.
			var actual = connectionString.ToConnectionDetails(Substitute.For<IConsoleWrapper>());

			// Assert.
			actual.ConnectionString.Should().Be(connectionString);
		}
	}
}
