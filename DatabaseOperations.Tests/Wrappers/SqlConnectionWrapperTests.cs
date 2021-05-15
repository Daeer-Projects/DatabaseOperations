using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.Wrappers
{
    public class SqlConnectionWrapperTests
	{
        [Fact]
        public void TestGetConnectionReturnsNonNullObject()
        {
            // Arrange.
            ISqlConnectionWrapper wrapper =
                new SqlConnectionWrapper(
                    "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;");

            // Act.
            var connection = wrapper.Get();

            // Assert.
            connection.Should().NotBeNull();
        }
	}
}
