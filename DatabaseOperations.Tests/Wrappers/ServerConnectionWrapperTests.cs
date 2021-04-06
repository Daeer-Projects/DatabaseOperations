using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.Wrappers
{
    public class ServerConnectionWrapperTests
	{
        [Fact]
        public void TestGetServerConnectionReturnsNotNullObject()
        {
            // Arrange.
            ISqlConnectionWrapper sqlConnection =
                new SqlConnectionWrapper(
                    "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;");
            IServerConnectionWrapper serverConnection = new ServerConnectionWrapper(sqlConnection);

            // Act.
            var connection = serverConnection.GetServerConnection();

            // Assert.
            connection.Should().NotBeNull();
        }
	}
}
