namespace DatabaseOperations.Tests.Wrappers
{
    using DatabaseOperations.Wrappers;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.Data.SqlClient;
    using Xunit;

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
            SqlConnection connection = wrapper.Get();

            // Assert.
            connection.Should()
                .NotBeNull();
        }
    }
}