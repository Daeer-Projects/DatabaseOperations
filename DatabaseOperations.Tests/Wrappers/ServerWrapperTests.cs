using DatabaseOperations.Interfaces;
using DatabaseOperations.Wrappers;
using FluentAssertions;
using Xunit;

namespace DatabaseOperations.Tests.Wrappers
{
    public class ServerWrapperTests
	{
        [Fact]
        public void TestGetServerReturnsNotNullObject()
        {
            // Arrange.
            ISqlConnectionWrapper sqlConnection =
                new SqlConnectionWrapper(
                    "server=127.0.0.1;database=Bananas;User Id=sa;Password=password;Connect Timeout=10;");
            IServerConnectionWrapper serverConnection = new ServerConnectionWrapper(sqlConnection);
            IServerWrapper server = new ServerWrapper(serverConnection);

            // Act.
            var actualServer = server.GetServer();

            // Assert.
            actualServer.Should().NotBeNull();
        }
	}
}
