using System;
using DatabaseOperations.Core.Extensions;
using DatabaseOperations.Core.Validators;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Core.Tests.Validators
{
    public class ConnectionArrayValidatorTests
    {
        [Fact]
		public void TestConnectionValidatorWithValidArrayReturnsTrue()
		{
			// Arrange.
			var connectionArray = new[] { "server=127.0.0.1", "database=Bananas", "User Id=sa", "Password=password", "Connect Timeout=10" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeTrue();
		}

		[Fact]
		public void TestConnectionValidatorWithEmptyArrayReturnsFalse()
		{
			// Arrange.
			var connectionArray = Array.Empty<string>();

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestConnectionValidatorWithArrayOfTwoItemsReturnsFalse()
		{
			// Arrange.
			string[] connectionArray = { "server=127.0.0.1", "database=Bananas" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Theory]
		[InlineData("something")]
		[InlineData("blah")]
		[InlineData("")]
		[InlineData("server")]
		[InlineData("SERVER")]
		public void TestConnectionValidatorWithInvalidServerItemReturnsFalse(string serverItem)
		{
			// Arrange.
			var connectionArray = new[] { serverItem, "database=Bananas", "User Id=sa", "Password=password", "Connect Timeout=10" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Theory]
		[InlineData("Bananas")]
		[InlineData("uh")]
		[InlineData("")]
		[InlineData("database")]
		[InlineData("DATABASE")]
		public void TestConnectionValidatorWithInvalidDatabaseItemReturnsFalse(string databaseItem)
		{
			// Arrange.
			var connectionArray = new[] { "server=127.0.0.1", databaseItem, "User Id=sa", "Password=password", "Connect Timeout=10" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestConnectionValidatorWithNullServerItemReturnsFalse()
		{
			// Arrange.
			var connectionArray = new[] { null, "Bananas", "User Id=sa", "Password=password", "Connect Timeout=10" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}

		[Fact]
		public void TestConnectionValidatorWithNullDatabaseItemReturnsFalse()
		{
			// Arrange.
			var connectionArray = new[] { "server=127.0.0.1", null, "User Id=sa", "Password=password", "Connect Timeout=10" };

			// Act.
			var results = connectionArray.CheckValidation(new ConnectionArrayValidator());

			// Assert.
			results.IsValid.Should().BeFalse();
		}
	}
}
